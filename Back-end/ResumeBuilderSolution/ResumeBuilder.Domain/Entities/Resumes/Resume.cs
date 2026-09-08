using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Entities.Identity;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Entities.Resumes;

public sealed class Resume : AuditableEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public ResumeStatus Status { get; set; }
    public ResumeSetting Settings { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }

    public User User { get; set; } = null!;
    public ICollection<ResumeSection>? ResumeSections { get; set; }
}
