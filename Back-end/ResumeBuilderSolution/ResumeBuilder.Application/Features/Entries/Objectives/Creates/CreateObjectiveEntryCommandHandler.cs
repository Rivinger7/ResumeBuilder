using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Creates;

public sealed class CreateObjectiveEntryCommandHandler(IObjectiveEntryRepository objectiveEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateObjectiveEntryCommand, CreateObjectiveEntryResponse>
{
    private readonly IObjectiveEntryRepository _objectiveEntryRepository = objectiveEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateObjectiveEntryResponse> Handle(CreateObjectiveEntryCommand request, CancellationToken cancellationToken)
    {
        int currentDisplayOrder = await _objectiveEntryRepository.GetCurrentDisplayOrderByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken) ?? 0;

        ObjectiveEntry objectiveEntry = new()
        {
            Id = Guid.NewGuid(),
            ResumeSectionId = request.ResumeSectionId,
            Title = request.Title,
            SubTitle = request.SubTitle,
            Description = request.Description,
            DisplayOrder = currentDisplayOrder + 1
        };

        await _objectiveEntryRepository.AddAsync(objectiveEntry, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateObjectiveEntryResponse();
    }
}