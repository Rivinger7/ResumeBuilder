using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateLanguageEntryInternalRequest(Guid Id, Optional<string?> LanguageName, Optional<string?> Proficiency);
