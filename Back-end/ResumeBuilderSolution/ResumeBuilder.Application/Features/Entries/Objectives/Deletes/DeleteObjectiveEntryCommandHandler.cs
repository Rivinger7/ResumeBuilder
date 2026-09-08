using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Deletes;

public sealed class DeleteObjectiveEntryCommandHandler(IObjectiveEntryRepository objectiveEntryRepository) : IRequestHandler<DeleteObjectiveEntryCommand, DeleteObjectiveEntryResponse>
{
    private readonly IObjectiveEntryRepository _objectiveEntryRepository = objectiveEntryRepository;

    public async Task<DeleteObjectiveEntryResponse> Handle(DeleteObjectiveEntryCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _objectiveEntryRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"Objective entry with ID {request.Id} not found.");
        }

        return new DeleteObjectiveEntryResponse();
    }
}
