using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Creates;

public sealed record CreateExperienceEntryCommand(Guid ResumeSectionId, string? CompanyName, string? Position, DateOnly? StartDate, DateOnly? EndDate, bool IsCurrent, string? Location, string? Description, bool IsByOrder) : IRequest<CreateExperienceEntryResponse>;