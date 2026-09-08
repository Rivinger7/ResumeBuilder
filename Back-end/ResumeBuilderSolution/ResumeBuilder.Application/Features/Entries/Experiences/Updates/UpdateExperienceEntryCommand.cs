using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Updates;

public sealed record UpdateExperienceEntryCommand(Guid Id, Optional<string?> CompanyName, Optional<string?> Position, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<bool?> IsCurrent, Optional<string?> Location, Optional<string?> Description, Optional<bool?> IsByOrder) : IRequest<UpdateExperienceEntryResponse>;
