using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateProjectEntryRequest(Optional<string?> Title, Optional<string?> SubTitle, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<string?> ProjectUrl, Optional<string?> RepositoryUrl, Optional<string?> Description);
