using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Deletes;

public sealed class DeleteExperienceEntryCommandHandler(IExperienceEntryRepository experienceEntryRepository) : IRequestHandler<DeleteExperienceEntryCommand, DeleteExperienceEntryResponse>
{
    private readonly IExperienceEntryRepository _experienceEntryRepository = experienceEntryRepository;

    public async Task<DeleteExperienceEntryResponse> Handle(DeleteExperienceEntryCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _experienceEntryRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"Experience entry with ID {request.Id} not found.");
        }

        return new DeleteExperienceEntryResponse();
    }
}
