using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Deletes;

public sealed record DeleteSummaryEntryCommand(Guid Id) : IRequest<DeleteSummaryEntryResponse>;