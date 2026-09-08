using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Educations.Deletes;

public sealed class DeleteEducationEntryCommandHandler(IEducationEntryRepository educationEntryRepository) : IRequestHandler<DeleteEducationEntryCommand, DeleteEducationEntryResponse>
{
    private readonly IEducationEntryRepository _educationEntryRepository = educationEntryRepository;

    public async Task<DeleteEducationEntryResponse> Handle(DeleteEducationEntryCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _educationEntryRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"Education entry with ID {request.Id} not found.");
        }

        return new DeleteEducationEntryResponse();
    }
}
