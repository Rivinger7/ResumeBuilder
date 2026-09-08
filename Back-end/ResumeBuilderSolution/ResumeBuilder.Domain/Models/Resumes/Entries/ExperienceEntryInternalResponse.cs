namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record ExperienceEntryInternalResponse(Guid Id, string? CompanyName, string? Position, DateOnly? StartDate, DateOnly? EndDate, bool IsCurrent, string? Location, string? Description, int DisplayOrder, bool IsByOrder);
