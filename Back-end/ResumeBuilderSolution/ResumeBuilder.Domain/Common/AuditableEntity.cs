using ResumeBuilder.Domain.Helpers;

namespace ResumeBuilder.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; } = CustomTimeProvider.UtcNowOffset;

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
}
