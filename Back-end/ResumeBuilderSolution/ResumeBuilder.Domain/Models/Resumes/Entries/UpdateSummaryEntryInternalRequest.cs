using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateSummaryEntryInternalRequest(Guid Id, Optional<string?> Summary);
