using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Projects.Updates;

public sealed class UpdateProjectEntryCommandHandler(IProjectEntryRepository projectEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProjectEntryCommand, UpdateProjectEntryResponse>
{
    private readonly IProjectEntryRepository _projectEntryRepository = projectEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateProjectEntryResponse> Handle(UpdateProjectEntryCommand request, CancellationToken cancellationToken)
    {
        UpdateProjectEntryInternalRequest updateProjectEntryInternalRequest = new(request.Id, request.Title, request.SubTitle, request.StartDate, request.EndDate, request.ProjectUrl, request.RepositoryUrl, request.Description);

        int rowsAffected = await _projectEntryRepository.UpdateContentByIdAsync(updateProjectEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Project entry not found");
        }

        return new UpdateProjectEntryResponse();
    }
}