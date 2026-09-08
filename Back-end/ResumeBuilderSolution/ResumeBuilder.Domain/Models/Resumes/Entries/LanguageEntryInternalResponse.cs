namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record LanguageEntryInternalResponse(Guid Id, string? LanguageName, string? Proficiency, int DisplayOrder);
