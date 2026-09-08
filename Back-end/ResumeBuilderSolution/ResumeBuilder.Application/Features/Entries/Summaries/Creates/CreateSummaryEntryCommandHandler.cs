using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Summaries.Creates;

public sealed class CreateSummaryEntryCommandHandler(ISummaryEntryRepository summaryEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateSummaryEntryCommand, CreateSummaryEntryResponse>
{
    private readonly ISummaryEntryRepository _summaryEntryRepository = summaryEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateSummaryEntryResponse> Handle(CreateSummaryEntryCommand request, CancellationToken cancellationToken)
    {
        int currentDisplayOrder = await _summaryEntryRepository.GetCurrentDisplayOrderByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken) ?? 0;

        SummaryEntry summaryEntry = new()
        {
            Id = Guid.NewGuid(),
            ResumeSectionId = request.ResumeSectionId,
            Summary = request.Summary,
            DisplayOrder = currentDisplayOrder + 1
        };

        await _summaryEntryRepository.AddAsync(summaryEntry, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateSummaryEntryResponse();
    }
}