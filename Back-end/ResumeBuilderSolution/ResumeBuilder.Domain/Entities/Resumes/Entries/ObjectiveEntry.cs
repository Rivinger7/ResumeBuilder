using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public sealed class ObjectiveEntry : BaseEntity, IDisplayOrderedEntry
{
    public Guid ResumeSectionId { get; set; }
    public string? Title { get; set; }
    public string? SubTitle { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }

    public ResumeSection ResumeSection { get; set; } = null!;
}
