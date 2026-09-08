using MediatR;
using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Identity;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Authentication;
using RefreshTokenEntity = ResumeBuilder.Domain.Entities.Identity.RefreshToken;

namespace ResumeBuilder.Application.Features.Authentication.Register;

public sealed class RegisterCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new ConflictException("Email is already existed.");
        }

        User user = new()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            FullName = request.FullName,
            AvatarUrl = "https://res.cloudinary.com/dofnn7sbx/image/upload/v1785418451/avatart-profile_zu6buv.png",
            Role = UserRole.User
        };



        JwtPayloadInternalRequest jwtPayload = new(user.Id, user.Role, user.Email, user.FullName);

        string accessToken = _jwtProvider.GenerateAccessTokenString(jwtPayload);
        string refreshToken = _jwtProvider.GenerateRefreshTokenString();

        RefreshTokenEntity newRefreshTokenEntity = new()
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiredAt = CustomTimeProvider.UtcNowOffset.AddDays(30),
            CreatedAt = CustomTimeProvider.UtcNowOffset,
            CreatedByIp = request.IpAddress,
            UserAgent = request.UserAgent,
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(accessToken, refreshToken);
    }
}
