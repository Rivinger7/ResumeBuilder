using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Helpers;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes;

public sealed class ResumeSection : AuditableEntity, IDisplayOrderedResumeSection
{
    public Guid ResumeId { get; set; }
    public ResumeSectionType Type { get; set; }
    public string Title => Type.ToString().ToSpacedWords();
    public int DisplayOrder { get; set; }

    public Resume Resume { get; set; } = null!;
    public ICollection<CertificateEntry>? CertificateEntries { get; set; }
    public ICollection<EducationEntry>? EducationEntries { get; set; }
    public ICollection<ObjectiveEntry>? ObjectiveEntries { get; set; }
    public ICollection<ExperienceEntry>? ExperienceEntries { get; set; }
    public ICollection<ProjectEntry>? ProjectEntries { get; set; }
    public ICollection<LanguageEntry>? LanguageEntries { get; set; }
    public ICollection<PersonalInformationEntry>? PersonalInformationEntries { get; set; }
    public ICollection<SummaryEntry>? SummaryEntries { get; set; }
    public ICollection<SkillEntry>? SkillEntries { get; set; }
}
