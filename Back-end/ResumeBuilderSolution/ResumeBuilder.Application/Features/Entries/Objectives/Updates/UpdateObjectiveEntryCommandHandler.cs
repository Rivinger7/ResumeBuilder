using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Objectives.Updates;

public sealed class UpdateObjectiveEntryCommandHandler(IObjectiveEntryRepository objectiveEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateObjectiveEntryCommand, UpdateObjectiveEntryResponse>
{
    private readonly IObjectiveEntryRepository _objectiveEntryRepository = objectiveEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateObjectiveEntryResponse> Handle(UpdateObjectiveEntryCommand request, CancellationToken cancellationToken)
    {
        UpdateObjectiveEntryInternalRequest updateObjectiveEntryInternalRequest = new(request.Id, request.Title, request.SubTitle, request.Description);

        int rowsAffected = await _objectiveEntryRepository.UpdateContentByIdAsync(updateObjectiveEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Objective entry not found");
        }

        return new UpdateObjectiveEntryResponse();
    }
}