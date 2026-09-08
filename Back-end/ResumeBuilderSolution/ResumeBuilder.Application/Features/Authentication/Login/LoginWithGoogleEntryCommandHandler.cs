using MediatR;
using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Identity;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Authentication;
using RefreshTokenEntity = ResumeBuilder.Domain.Entities.Identity.RefreshToken;

namespace ResumeBuilder.Application.Features.Authentication.Login;

public sealed class LoginWithGoogleEntryCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IGoogleValidation googleValidation, IPasswordHasher passwordHasher, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork) : IRequestHandler<LoginWithGoogleEntryCommand, LoginWithGoogleEntryResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IGoogleValidation _googleValidation = googleValidation;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<LoginWithGoogleEntryResponse> Handle(LoginWithGoogleEntryCommand request, CancellationToken cancellationToken)
    {
        JwtGooglePayloadInternalResponse jwtGooglePayloadInternalResponse = await _googleValidation.ValidateGoogleTokenAsync(request.IdToken);

        User? existingUser = await _userRepository.GetByEmailAsync(jwtGooglePayloadInternalResponse.Email, cancellationToken);
        bool isNewUser = existingUser is null;

        User user = existingUser ?? new()
        {
            Id = Guid.NewGuid(),
            Email = jwtGooglePayloadInternalResponse.Email,
            PasswordHash = _passwordHasher.Hash(jwtGooglePayloadInternalResponse.Sub + jwtGooglePayloadInternalResponse.Email),
            FullName = jwtGooglePayloadInternalResponse.FullName,
            AvatarUrl = jwtGooglePayloadInternalResponse.Picture,
            Role = UserRole.User,
        };

        JwtPayloadInternalRequest jwtPayloadInternalRequest = new(user.Id, user.Role, user.Email, user.FullName);

        string accessToken = _jwtProvider.GenerateAccessTokenString(jwtPayloadInternalRequest);
        string refreshToken = _jwtProvider.GenerateRefreshTokenString();

        RefreshTokenEntity refreshTokenEntity = new()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiredAt = CustomTimeProvider.UtcNowOffset.AddDays(30),
            CreatedAt = CustomTimeProvider.UtcNowOffset,
            CreatedByIp = request.IpAddress,
            UserAgent = request.UserAgent,
        };

        if (isNewUser)
        {
            await _userRepository.AddAsync(user, cancellationToken);
        }
        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginWithGoogleEntryResponse(accessToken, refreshToken);
    }
}