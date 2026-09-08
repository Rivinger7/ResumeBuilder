using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Skills.Updates;

public sealed class UpdateSkillEntryStyleCommandHandler(ISkillEntryRepository skillEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateSkillEntryStyleCommand, UpdateSkillEntryStyleResponse>
{
    private readonly ISkillEntryRepository _skillEntryRepository = skillEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateSkillEntryStyleResponse> Handle(UpdateSkillEntryStyleCommand request, CancellationToken cancellationToken)
    {
        UpdateSkillEntryStyleInternalRequest updateSkillEntryStyleInternalRequest = new(request.Id, request.SkillLayout, request.SkillGridColumn, request.SkillRowSpacing, request.IsStartRowsWithBullet, request.SubinfoStyle);

        int rowsAffected = await _skillEntryRepository.UpdateStyleByIdAsync(updateSkillEntryStyleInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Skill entry not found");
        }

        return new UpdateSkillEntryStyleResponse();
    }
}
