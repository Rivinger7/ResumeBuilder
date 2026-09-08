using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public sealed class ExperienceEntry : BaseEntity, IDisplayOrderedEntry
{
    public Guid ResumeSectionId { get; set; }
    public string? CompanyName { get; set; }
    public string? Position { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }

    public bool IsByOrder { get; set; }

    public ResumeSection ResumeSection { get; set; } = null!;
}
