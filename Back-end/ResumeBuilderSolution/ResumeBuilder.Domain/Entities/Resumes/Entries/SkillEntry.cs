using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public sealed class SkillEntry : BaseEntity, IDisplayOrderedEntry
{
    public Guid ResumeSectionId { get; set; }
    public string? SkillName { get; set; }
    public string? Description { get; set; }
    public string? SkillLevel { get; set; }
    public int DisplayOrder { get; set; }

    public LayoutType SkillLayout { get; set; }
    public int? SkillGridColumn { get; set; }
    public string? SkillRowSpacing { get; set; }
    public bool IsStartRowsWithBullet { get; set; }
    public string? SubinfoStyle { get; set; }

    public ResumeSection ResumeSection { get; set; } = null!;
}
