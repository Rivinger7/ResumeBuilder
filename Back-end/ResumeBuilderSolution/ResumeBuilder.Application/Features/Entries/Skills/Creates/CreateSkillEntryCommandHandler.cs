using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Skills.Creates;

public sealed class CreateSkillEntryCommandHandler(ISkillEntryRepository skillEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateSkillEntryCommand, CreateSkillEntryResponse>
{
    private readonly ISkillEntryRepository _skillEntryRepository = skillEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateSkillEntryResponse> Handle(CreateSkillEntryCommand request, CancellationToken cancellationToken)
    {
        int currentDisplayOrder = await _skillEntryRepository.GetCurrentDisplayOrderByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken) ?? 0;

        SkillEntry skillEntry = new()
        {
            Id = Guid.NewGuid(),
            ResumeSectionId = request.ResumeSectionId,
            SkillName = request.SkillName,
            Description = request.Description,
            SkillLevel = request.SkillLevel,
            DisplayOrder = currentDisplayOrder + 1,
            SkillLayout = request.SkillLayout,
            SkillGridColumn = request.SkillGridColumn,
            SkillRowSpacing = request.SkillRowSpacing,
            IsStartRowsWithBullet = request.IsStartRowsWithBullet,
            SubinfoStyle = request.SubinfoStyle
        };

        await _skillEntryRepository.AddAsync(skillEntry, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateSkillEntryResponse();
    }
}
