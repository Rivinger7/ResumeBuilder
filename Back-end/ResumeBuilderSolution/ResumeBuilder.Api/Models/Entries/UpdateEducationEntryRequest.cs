using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateEducationEntryRequest(Optional<string?> SchoolName, Optional<string?> Degree, Optional<string?> Major, Optional<decimal?> GPA, Optional<bool?> IsCurrent, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<string?> Location, Optional<string?> Description, Optional<bool?> IsByOrder);
