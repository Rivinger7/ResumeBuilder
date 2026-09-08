using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.Educations.Updates;

public sealed record UpdateEducationEntryCommand(Guid Id, Optional<string?> SchoolName, Optional<string?> Degree, Optional<string?> Major, Optional<decimal?> GPA, Optional<bool?> IsCurrent, Optional<DateOnly?> StartDate, Optional<DateOnly?> EndDate, Optional<string?> Location, Optional<string?> Description, Optional<bool?> IsByOrder) : IRequest<UpdateEducationEntryResponse>;
