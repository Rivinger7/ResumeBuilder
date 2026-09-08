using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Creates;

public sealed record CreateSummaryEntryCommand(Guid ResumeSectionId, string? Summary) : IRequest<CreateSummaryEntryResponse>;