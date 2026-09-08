using MediatR;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.ResumeSections.Creates;

public sealed class CreateResumeSectionCommandHandler(IResumeSectionRepository resumeSectionRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateResumeSectionCommand, CreateResumeSectionResponse>
{
    private readonly IResumeSectionRepository _resumeSectionRepository = resumeSectionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateResumeSectionResponse> Handle(CreateResumeSectionCommand request, CancellationToken cancellationToken)
    {
        if (await _resumeSectionRepository.IsResumeSectionExistedByType(request.ResumeId, request.Type, cancellationToken))
        {
            throw new ConflictException($"A resume section with the same type '{request.Type}' already exists for this resume.");
        }

        Guid resumeSectionId = Guid.NewGuid();
        ResumeSection resumeSection = new()
        {
            Id = resumeSectionId,
            ResumeId = request.ResumeId,
            Type = request.Type,
            DisplayOrder = request.DisplayOrder,
        };

        switch (request.Type)
        {
            case ResumeSectionType.Certificates:
                resumeSection.CertificateEntries = [new CertificateEntry { Id = Guid.NewGuid(), ResumeSectionId = resumeSectionId, DisplayOrder = 1 }];
                break;
            case ResumeSectionType.Education:
                resumeSection.EducationEntries = [new EducationEntry { Id = Guid.NewGuid(), ResumeSectionId = resumeSectionId, DisplayOrder = 1 }];
                break;
            case ResumeSectionType.Objective:
                resumeSection.ObjectiveEntries = [new ObjectiveEntry { Id = Guid.NewGuid(), ResumeSectionId = resumeSectionId, DisplayOrder = 1 }];
                break;
            case ResumeSectionType.Experience:
                resumeSection.ExperienceEntries = [new ExperienceEntry { Id = Guid.NewGuid(), ResumeSectionId = resumeSectionId, DisplayOrder = 1 }];
                break;
            case ResumeSectionType.Projects:
                resumeSection.ProjectEntries = [new ProjectEntry { Id = Guid.NewGuid(), ResumeSectionId = resumeSectionId, DisplayOrder = 1 }];
                break;
            case ResumeSectionType.Languages:
                resumeSection.LanguageEntries = [new LanguageEntry { Id = Guid.NewGuid(), ResumeSectionId = resumeSectionId, DisplayOrder = 1 }];
                break;
            case ResumeSectionType.Summary:
                resumeSection.SummaryEntries = [new SummaryEntry { Id = Guid.NewGuid(), ResumeSectionId = resumeSectionId, DisplayOrder = 1 }];
                break;
        }

        await _resumeSectionRepository.AddAsync(resumeSection, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateResumeSectionResponse(resumeSectionId);
    }
}