using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Skills.Updates;

public sealed class UpdateSkillEntryCommandHandler(ISkillEntryRepository skillEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateSkillEntryCommand, UpdateSkillEntryResponse>
{
    private readonly ISkillEntryRepository _skillEntryRepository = skillEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateSkillEntryResponse> Handle(UpdateSkillEntryCommand request, CancellationToken cancellationToken)
    {
        UpdateSkillEntryInternalRequest updateSkillEntryInternalRequest = new(request.Id, request.SkillName, request.Description, request.SkillLevel);

        int rowsAffected = await _skillEntryRepository.UpdateContentByIdAsync(updateSkillEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Skill entry not found");
        }

        return new UpdateSkillEntryResponse();
    }
}
