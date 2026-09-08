using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Skills.Deletes;

public sealed record DeleteSkillEntryCommand(Guid Id) : IRequest<DeleteSkillEntryResponse>;
