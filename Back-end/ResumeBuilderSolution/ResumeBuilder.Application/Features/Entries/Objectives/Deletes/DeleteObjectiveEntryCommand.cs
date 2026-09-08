using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Deletes;

public sealed record DeleteObjectiveEntryCommand(Guid Id) : IRequest<DeleteObjectiveEntryResponse>;