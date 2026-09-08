namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record SkillEntryInternalResponse(Guid Id, string? SkillName, string? Description, string? SkillLevel, int DisplayOrder);
