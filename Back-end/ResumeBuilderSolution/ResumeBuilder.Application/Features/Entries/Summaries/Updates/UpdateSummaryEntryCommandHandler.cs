using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Updates;

public sealed class UpdateSummaryEntryCommandHandler(ISummaryEntryRepository summaryEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateSummaryEntryCommand, UpdateSummaryEntryResponse>
{
    private readonly ISummaryEntryRepository _summaryEntryRepository = summaryEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateSummaryEntryResponse> Handle(UpdateSummaryEntryCommand request, CancellationToken cancellationToken)
    {
        UpdateSummaryEntryInternalRequest updateSummaryEntryInternalRequest = new(request.Id, request.Summary);

        int rowsAffected = await _summaryEntryRepository.UpdateContentByIdAsync(updateSummaryEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Summary entry not found");
        }

        return new UpdateSummaryEntryResponse();
    }
}