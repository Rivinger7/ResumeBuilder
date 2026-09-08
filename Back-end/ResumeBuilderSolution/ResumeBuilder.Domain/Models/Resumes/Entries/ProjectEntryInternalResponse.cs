namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record ProjectEntryInternalResponse(Guid Id, string? Title, string? SubTitle, DateOnly? StartDate, DateOnly? EndDate, string? ProjectUrl, string? RepositoryUrl, string? Description, int DisplayOrder);
