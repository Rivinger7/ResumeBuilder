using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Educations.Creates;

public sealed class CreateEducationEntryCommandHandler(IEducationEntryRepository educationEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateEducationEntryCommand, CreateEducationEntryResponse>
{
    private readonly IEducationEntryRepository _educationEntryRepository = educationEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateEducationEntryResponse> Handle(CreateEducationEntryCommand request, CancellationToken cancellationToken)
    {
        int currentDisplayOrder = await _educationEntryRepository.GetCurrentDisplayOrderByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken) ?? 0;
        EducationEntry educationEntry = new()
        {
            Id = Guid.NewGuid(),
            ResumeSectionId = request.ResumeSectionId,
            SchoolName = request.SchoolName,
            Degree = request.Degree,
            Major = request.Major,
            GPA = request.GPA,
            IsCurrent = request.IsCurrent,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Location = request.Location,
            Description = request.Description,
            DisplayOrder = currentDisplayOrder + 1,
            IsByOrder = request.IsByOrder
        };

        await _educationEntryRepository.AddAsync(educationEntry, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateEducationEntryResponse();
    }
}