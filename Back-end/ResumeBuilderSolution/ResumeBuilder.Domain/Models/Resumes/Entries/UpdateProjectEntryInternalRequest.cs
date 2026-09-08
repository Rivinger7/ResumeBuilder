using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateProjectEntryInternalRequest(Guid Id, Optional<string?> Title, Optional<string?> SubTitle, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<string?> ProjectUrl, Optional<string?> RepositoryUrl, Optional<string?> Description);
