using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Projects.Deletes;

public sealed class DeleteProjectEntryCommandHandler(IProjectEntryRepository projectEntryRepository) : IRequestHandler<DeleteProjectEntryCommand, DeleteProjectEntryResponse>
{
    private readonly IProjectEntryRepository _projectEntryRepository = projectEntryRepository;

    public async Task<DeleteProjectEntryResponse> Handle(DeleteProjectEntryCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _projectEntryRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"Project entry with ID {request.Id} not found.");
        }

        return new DeleteProjectEntryResponse();
    }
}
