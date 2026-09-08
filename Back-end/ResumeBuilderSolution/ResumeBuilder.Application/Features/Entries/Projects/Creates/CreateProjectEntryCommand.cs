using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Projects.Creates;

public sealed record CreateProjectEntryCommand(Guid ResumeSectionId, string? Title, string? SubTitle, DateOnly? StartDate, DateOnly? EndDate, string? ProjectUrl, string? RepositoryUrl, string? Description) : IRequest<CreateProjectEntryResponse>;