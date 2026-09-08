namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record EducationEntryInternalResponse(Guid Id, string? SchoolName, string? Degree, string? Major, decimal? GPA, bool IsCurrent, DateOnly? StartDate, DateOnly? EndDate, string? Location, string? Description, int DisplayOrder, bool IsByOrder);
