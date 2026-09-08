using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public class SummaryEntry : BaseEntity, IDisplayOrderedEntry
{
    public Guid ResumeSectionId { get; set; }
    public string? Summary { get; set; }
    public int DisplayOrder { get; set; }

    public ResumeSection ResumeSection { get; set; } = null!;
}
