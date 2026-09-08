using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Skills.Deletes;

public sealed class DeleteSkillEntryCommandHandler(ISkillEntryRepository skillEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteSkillEntryCommand, DeleteSkillEntryResponse>
{
    private readonly ISkillEntryRepository _skillEntryRepository = skillEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteSkillEntryResponse> Handle(DeleteSkillEntryCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _skillEntryRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Skill entry not found");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DeleteSkillEntryResponse();
    }
}
