using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateLanguageEntryRequest(Optional<string?> LanguageName, Optional<string?> Proficiency);
