using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Deletes;

public sealed class DeleteSummaryEntryCommandHandler(ISummaryEntryRepository summaryEntryRepository) : IRequestHandler<DeleteSummaryEntryCommand, DeleteSummaryEntryResponse>
{
    private readonly ISummaryEntryRepository _summaryEntryRepository = summaryEntryRepository;

    public async Task<DeleteSummaryEntryResponse> Handle(DeleteSummaryEntryCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _summaryEntryRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"Summary entry with ID {request.Id} not found.");
        }

        return new DeleteSummaryEntryResponse();
    }
}
