using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Helpers;

namespace ResumeBuilder.Domain.Entities.Identity;

public sealed class RefreshToken : AuditableEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTimeOffset ExpiredAt { get; set; }
    public bool IsExpired => CustomTimeProvider.GetUtcPlus7TimeOffset() > ExpiredAt;
    public DateTimeOffset? RevokedAt { get; set; }
    public bool IsRevoked => RevokedAt is not null;
    public bool IsActive => !IsRevoked && !IsExpired;
    public string? ReplacedByToken { get; set; }
    public string? CreatedByIp { get; set; }
    public string? RevokedByIp { get; set; }
    public string? UserAgent { get; set; }

    public User User { get; set; } = null!;
}
