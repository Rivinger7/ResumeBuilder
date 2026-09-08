using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

internal class RefreshTokenRepository(ApplicationDbContext context) : Repository<RefreshToken>(context), IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task<List<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Where(x =>
                x.UserId == userId &&
                x.RevokedAt == null &&
                x.ExpiredAt > CustomTimeProvider.UtcNowOffset)
            .ToListAsync(cancellationToken);
    }

    public async Task RevokeAllByUserIdAsync(Guid userId, string? revokedByIp, CancellationToken cancellationToken = default)
    {
        List<RefreshToken> activeTokens = await _context.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpiredAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        DateTimeOffset revokedAt = CustomTimeProvider.UtcNowOffset;

        foreach (RefreshToken token in activeTokens)
        {
            token.RevokedAt = revokedAt;
            token.RevokedByIp = revokedByIp;
        }
    }

    public async Task RevokeOldestIfExceedLimitAsync(Guid userId, int maxActiveSessions, string? revokedByIp, CancellationToken cancellationToken = default)
    {
        List<RefreshToken> activeTokens = await _context.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpiredAt > CustomTimeProvider.UtcNowOffset)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        int excessCount = activeTokens.Count - (maxActiveSessions - 1); // Minus 1 to save for the new token

        if (excessCount > 0)
        {
            DateTimeOffset revokedAt = CustomTimeProvider.UtcNowOffset;

            foreach (RefreshToken token in activeTokens.Take(excessCount))
            {
                token.RevokedAt = revokedAt;
                token.RevokedByIp = revokedByIp;
            }
        }
    }
}
