using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Educations.Updates;

public sealed class UpdateEducationEntryCommandHandler(IEducationEntryRepository educationEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateEducationEntryCommand, UpdateEducationEntryResponse>
{
    private readonly IEducationEntryRepository _educationEntryRepository = educationEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateEducationEntryResponse> Handle(UpdateEducationEntryCommand request, CancellationToken cancellationToken)
    {
        UpdateEducationEntryInternalRequest updateEducationEntryInternalRequest = new(request.Id, request.SchoolName, request.Degree, request.Major, request.GPA, request.IsCurrent, request.StartDate, request.EndDate, request.Location, request.Description, request.IsByOrder);

        int rowsAffected = await _educationEntryRepository.UpdateContentByIdAsync(updateEducationEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Education entry not found");
        }

        return new UpdateEducationEntryResponse();
    }
}