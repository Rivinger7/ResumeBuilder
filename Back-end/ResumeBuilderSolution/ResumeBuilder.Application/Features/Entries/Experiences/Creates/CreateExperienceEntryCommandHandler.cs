using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Creates;

public sealed class CreateExperienceEntryCommandHandler(IExperienceEntryRepository experienceEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateExperienceEntryCommand, CreateExperienceEntryResponse>
{
    private readonly IExperienceEntryRepository _experienceEntryRepository = experienceEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateExperienceEntryResponse> Handle(CreateExperienceEntryCommand request, CancellationToken cancellationToken)
    {
        int currentDisplayOrder = await _experienceEntryRepository.GetCurrentDisplayOrderByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken) ?? 0;

        ExperienceEntry newExperienceEntry = new()
        {
            Id = Guid.NewGuid(),
            ResumeSectionId = request.ResumeSectionId,
            CompanyName = request.CompanyName,
            Position = request.Position,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrent = request.IsCurrent,
            Location = request.Location,
            Description = request.Description,
            DisplayOrder = currentDisplayOrder + 1
        };

        await _experienceEntryRepository.AddAsync(newExperienceEntry, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateExperienceEntryResponse();
    }
}