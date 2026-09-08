using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Educations.Creates;

public sealed record CreateEducationEntryCommand(Guid ResumeSectionId, string? SchoolName, string? Degree, string? Major, decimal? GPA, bool IsCurrent, DateOnly? StartDate, DateOnly? EndDate, string? Location, string? Description, bool IsByOrder) : IRequest<CreateEducationEntryResponse>;