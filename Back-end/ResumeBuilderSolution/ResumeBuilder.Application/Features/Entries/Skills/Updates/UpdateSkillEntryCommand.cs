using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.Skills.Updates;

public sealed record UpdateSkillEntryCommand(Guid Id, Optional<string?> SkillName, Optional<string?> Description, Optional<string?> SkillLevel) : IRequest<UpdateSkillEntryResponse>;
