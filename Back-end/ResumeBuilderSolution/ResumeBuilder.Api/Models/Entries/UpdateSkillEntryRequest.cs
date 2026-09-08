using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateSkillEntryRequest(Optional<string?> SkillName, Optional<string?> Description, Optional<string?> SkillLevel);
