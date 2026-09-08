using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Domain.Models.Resumes;

public sealed record ResumeSectionInternalResponse(Guid Id, ResumeSectionType Type, string Title, int DisplayOrder, IEnumerable<CertificateEntryInternalResponse>? CertificateEntries, IEnumerable<EducationEntryInternalResponse>? EducationEntries, IEnumerable<ObjectiveEntryInternalResponse>? ObjectiveEntries, IEnumerable<ExperienceEntryInternalResponse>? ExperienceEntries, IEnumerable<ProjectEntryInternalResponse>? ProjectEntries, IEnumerable<LanguageEntryInternalResponse>? LanguageEntries, IEnumerable<PersonalInformationEntryInternalResponse>? PersonalInformationEntries, IEnumerable<SummaryEntryInternalResponse>? SummaryEntries, IEnumerable<SkillEntryInternalResponse>? SkillEntries);
