using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Deletes;

public sealed record DeleteExperienceEntryCommand(Guid Id) : IRequest<DeleteExperienceEntryResponse>;