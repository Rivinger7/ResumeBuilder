using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Updates;

public sealed record UpdateSummaryEntryCommand(Guid Id, Optional<string?> Summary) : IRequest<UpdateSummaryEntryResponse>;
