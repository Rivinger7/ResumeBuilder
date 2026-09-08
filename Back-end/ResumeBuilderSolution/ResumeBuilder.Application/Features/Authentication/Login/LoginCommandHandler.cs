using Mapster;
using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Identity;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Authentication;
using RefreshTokenEntity = ResumeBuilder.Domain.Entities.Identity.RefreshToken;

namespace ResumeBuilder.Application.Features.Authentication.Login;

public sealed class LoginCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    private const int _maxActiveSessionsPerUser = 5;

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        JwtPayloadWithPasswordHashInternalRequest? userInfo = await _userRepository.GetJwtPayloadWithPasswordHashByEmailAsync(request.Email, cancellationToken) ?? throw new UnauthorizedException("Invalid email or password");

        bool isPasswordVerified = _passwordHasher.Verify(request.Password, userInfo.PasswordHash);
        if (!isPasswordVerified)
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        await _refreshTokenRepository.RevokeOldestIfExceedLimitAsync(userInfo.UserId, _maxActiveSessionsPerUser, request.IpAddress, cancellationToken);

        JwtPayloadInternalRequest jwtPayload = userInfo.Adapt<JwtPayloadInternalRequest>();

        string accessToken = _jwtProvider.GenerateAccessTokenString(jwtPayload);
        string refreshToken = _jwtProvider.GenerateRefreshTokenString();

        RefreshTokenEntity newRefreshTokenEntity = new()
        {
            UserId = userInfo.UserId,
            Token = refreshToken,
            ExpiredAt = CustomTimeProvider.UtcNowOffset.AddDays(30),
            CreatedByIp = request.IpAddress,
            UserAgent = request.UserAgent,
        };

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken, refreshToken);
    }
}