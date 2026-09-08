using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Projects.Creates;

public sealed class CreateProjectEntryCommandHandler(IProjectEntryRepository projectEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateProjectEntryCommand, CreateProjectEntryResponse>
{
    private readonly IProjectEntryRepository _projectEntryRepository = projectEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateProjectEntryResponse> Handle(CreateProjectEntryCommand request, CancellationToken cancellationToken)
    {
        int currentDisplayOrder = await _projectEntryRepository.GetCurrentDisplayOrderByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken) ?? 0;

        ProjectEntry projectEntry = new()
        {
            Id = Guid.NewGuid(),
            ResumeSectionId = request.ResumeSectionId,
            Title = request.Title,
            SubTitle = request.SubTitle,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ProjectUrl = request.ProjectUrl,
            Description = request.Description,
            DisplayOrder = currentDisplayOrder + 1
        };

        await _projectEntryRepository.AddAsync(projectEntry, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateProjectEntryResponse();
    }
}