using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Identity;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Authentication;
using RefreshTokenEntity = ResumeBuilder.Domain.Entities.Identity.RefreshToken;

namespace ResumeBuilder.Application.Features.Authentication.RefreshToken;

public sealed class RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IJwtProvider jwtProvider, IUnitOfWork unitOfWork) : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        RefreshTokenEntity refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.OldRefreshToken, cancellationToken) ?? throw new UnauthorizedException("Invalid refresh token");
        // Check if the token is revoked, if so there is attacker attempt to hack the account
        if (refreshTokenEntity.IsRevoked)
        {
            await _refreshTokenRepository.RevokeAllByUserIdAsync(refreshTokenEntity.UserId, request.IpAddress, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            throw new UnauthorizedException("Refresh token has been revoked. Please login again.");
        }
        if (refreshTokenEntity.IsExpired)
        {
            throw new UnauthorizedException("Refresh token has expired");
        }

        JwtPayloadInternalRequest jwtPayload = await _userRepository.GetJwtPayloadByIdAsync(refreshTokenEntity.UserId, cancellationToken) ?? throw new UnauthorizedException("User not found");

        string accessToken = _jwtProvider.GenerateAccessTokenString(jwtPayload);
        string newRefreshToken = _jwtProvider.GenerateRefreshTokenString();

        RefreshTokenEntity newRefreshTokenEntity = new()
        {
            UserId = refreshTokenEntity.UserId,
            Token = newRefreshToken,
            ExpiredAt = CustomTimeProvider.UtcNowOffset.AddDays(30),
            CreatedByIp = request.IpAddress,
            UserAgent = request.UserAgent,
        };

        // Revoke refresh old token
        refreshTokenEntity.RevokedAt = CustomTimeProvider.UtcNowOffset;
        refreshTokenEntity.RevokedByIp = request.IpAddress;
        refreshTokenEntity.ReplacedByToken = newRefreshTokenEntity.Token;

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse(accessToken, newRefreshToken);
    }
}