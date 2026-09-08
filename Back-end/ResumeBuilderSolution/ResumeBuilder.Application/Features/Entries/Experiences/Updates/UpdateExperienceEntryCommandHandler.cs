using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Experiences.Updates;

public sealed class UpdateExperienceEntryCommandHandler(IExperienceEntryRepository experienceEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateExperienceEntryCommand, UpdateExperienceEntryResponse>
{
    private readonly IExperienceEntryRepository _experienceEntryRepository = experienceEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateExperienceEntryResponse> Handle(UpdateExperienceEntryCommand request, CancellationToken cancellationToken)
    {
        UpdateExperienceEntryInternalRequest updateExperienceEntryInternalRequest = new(request.Id, request.CompanyName, request.Position, request.StartDate, request.EndDate, request.IsCurrent, request.Location, request.Description, request.IsByOrder);

        int rowsAffected = await _experienceEntryRepository.UpdateContentByIdAsync(updateExperienceEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Experience entry not found");
        }

        return new UpdateExperienceEntryResponse();
    }
}