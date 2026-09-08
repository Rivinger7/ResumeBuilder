using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Projects.Deletes;

public sealed record DeleteProjectEntryCommand(Guid Id) : IRequest<DeleteProjectEntryResponse>;