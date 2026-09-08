using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateExperienceEntryRequestt(Optional<string?> CompanyName, Optional<string?> Position, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<bool?> IsCurrent, Optional<string?> Location, Optional<string?> Description, Optional<bool?> IsByOrder);
