namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record ObjectiveEntryInternalResponse(Guid Id, string? Title, string? SubTitle, string? Description, int DisplayOrder);
