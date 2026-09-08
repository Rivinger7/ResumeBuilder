using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Updates;

public sealed record UpdateObjectiveEntryCommand(Guid Id, Optional<string?> Title, Optional<string?> SubTitle, Optional<string?> Description) : IRequest<UpdateObjectiveEntryResponse>;
