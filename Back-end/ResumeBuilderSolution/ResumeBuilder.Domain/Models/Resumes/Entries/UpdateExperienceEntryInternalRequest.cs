using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateExperienceEntryInternalRequest(Guid Id, Optional<string?> CompanyName, Optional<string?> Position, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<bool?> IsCurrent, Optional<string?> Location, Optional<string?> Description, Optional<bool?> IsByOrder);
