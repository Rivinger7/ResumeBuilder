namespace ResumeBuilder.Domain.Models.Resumes;

public sealed record CreateResumeInternalRequest(Guid UserId, string Title, string? Description);
