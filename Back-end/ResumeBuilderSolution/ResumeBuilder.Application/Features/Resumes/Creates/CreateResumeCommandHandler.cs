using MediatR;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Resumes.Creates;

public sealed class CreateResumeCommandHandler(IResumeRepository resumeRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateResumeCommand, CreateResumeResponse>
{
    private readonly IResumeRepository _resumeRepository = resumeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateResumeResponse> Handle(CreateResumeCommand request, CancellationToken cancellationToken)
    {
        if (await _resumeRepository.IsResumeExistedByTitle(request.Title, request.UserId, cancellationToken))
        {
            throw new ConflictException("A resume with the same title already exists.");
        }

        Guid resumeId = Guid.NewGuid();
        Guid resumeSectionId = Guid.NewGuid();
        Resume resume = new()
        {
            Id = resumeId,
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            Settings = new ResumeSetting
            {
                Id = Guid.NewGuid(),
                ResumeId = resumeId,
                Column = 1,
                Font = "Source Sans Pro",
                FontSize = 12F,
                LineHeight = 1.4F,
                SpaceBetweenElements = 20,
                LeftRightMargin = 15,
                SectionTitleFontSize = 14,
                SectionTitleColor = "#355c7d",
                BackgroundColorHeader = "#355C7D",
                IsPagenNumbersEnabled = false,
            },
            ResumeSections = [new ResumeSection {
                Id = Guid.NewGuid(),
                ResumeId = resumeSectionId,
                Type = ResumeSectionType.PersonalInformation,
                DisplayOrder = 1,
                PersonalInformationEntries = [new  PersonalInformationEntry {
                    Id = Guid.NewGuid(),
                    ResumeSectionId = resumeSectionId,
                    PersonalInformationTextAlignment = TextAlignmentType.Center,
                    PersonalInformationLayout = LayoutType.Rows,
                    PersonalInformationMarkerStyle = MarkerStyleType.Icon,
                    PersonalInformationIconStyle = IconStyleType.Default,
                    IsTitleProfessionalTitleOnSameLine = false,
                }]
            }],
        };

        await _resumeRepository.AddAsync(resume, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateResumeResponse(resumeId);
    }
}