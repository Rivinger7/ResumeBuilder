using MediatR;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using RefreshTokenEntity = ResumeBuilder.Domain.Entities.Identity.RefreshToken;

namespace ResumeBuilder.Application.Features.Authentication.Logout;

public sealed class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, LogoutResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        RefreshTokenEntity? refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        // Không tìm thấy token, hoặc token đã revoke/expired từ trước
        // -> vẫn coi là logout thành công (idempotent), không throw lỗi
        if (refreshTokenEntity is null || refreshTokenEntity.IsRevoked)
        {
            return new LogoutResponse();
        }

        refreshTokenEntity.RevokedAt = CustomTimeProvider.UtcNowOffset;
        refreshTokenEntity.RevokedByIp = request.IpAddress;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LogoutResponse();
    }
}