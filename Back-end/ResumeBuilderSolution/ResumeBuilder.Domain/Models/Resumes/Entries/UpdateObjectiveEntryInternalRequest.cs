using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateObjectiveEntryInternalRequest(Guid Id, Optional<string?> Title, Optional<string?> SubTitle, Optional<string?> Description);
