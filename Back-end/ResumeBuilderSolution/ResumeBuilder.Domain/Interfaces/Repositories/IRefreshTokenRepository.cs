using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<List<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task RevokeAllByUserIdAsync(Guid userId, string? revokedByIp, CancellationToken cancellationToken = default);
    Task RevokeOldestIfExceedLimitAsync(Guid userId, int maxActiveSessions, string? revokedByIp, CancellationToken cancellationToken = default);
}
