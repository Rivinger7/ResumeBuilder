using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateSkillEntryInternalRequest(Guid Id, Optional<string?> SkillName, Optional<string?> Description, Optional<string?> SkillLevel);
