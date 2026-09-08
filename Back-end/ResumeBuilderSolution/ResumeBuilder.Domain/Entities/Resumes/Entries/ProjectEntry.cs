using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public sealed class ProjectEntry : BaseEntity, IDisplayOrderedEntry
{
    public Guid ResumeSectionId { get; set; }
    public string? Title { get; set; }
    public string? SubTitle { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? ProjectUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }

    public ResumeSection ResumeSection { get; set; } = null!;
}
