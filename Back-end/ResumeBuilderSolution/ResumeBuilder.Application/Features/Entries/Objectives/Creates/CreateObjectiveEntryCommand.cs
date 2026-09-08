using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Creates;

public sealed record CreateObjectiveEntryCommand(Guid ResumeSectionId, string? Title, string? SubTitle, string? Description) : IRequest<CreateObjectiveEntryResponse>;