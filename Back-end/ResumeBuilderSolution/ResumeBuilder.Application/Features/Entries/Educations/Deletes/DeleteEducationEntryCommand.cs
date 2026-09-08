using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Educations.Deletes;

public sealed record DeleteEducationEntryCommand(Guid Id) : IRequest<DeleteEducationEntryResponse>;