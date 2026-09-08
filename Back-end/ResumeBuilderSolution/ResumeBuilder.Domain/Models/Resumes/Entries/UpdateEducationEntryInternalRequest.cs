using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateEducationEntryInternalRequest(Guid Id, Optional<string?> SchoolName, Optional<string?> Degree, Optional<string?> Major, Optional<decimal?> GPA, Optional<bool?> IsCurrent, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<string?> Location, Optional<string?> Description, Optional<bool?> IsByOrder);
