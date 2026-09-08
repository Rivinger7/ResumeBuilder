using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Updates;

public sealed class UpdatePersonalInformationEntryCommandHandler(IPersonalInformationEntryRepository personalInformationEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonalInformationEntryCommand, UpdatePersonalInformationEntryResponse>
{
    private readonly IPersonalInformationEntryRepository _personalInformationEntryRepository = personalInformationEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdatePersonalInformationEntryResponse> Handle(UpdatePersonalInformationEntryCommand request, CancellationToken cancellationToken)
    {
        UpdatePersonalInformationEntryInternalRequest updatePersonalInformationEntryInternalRequest = new(request.Id, request.FullName, request.ProfessionalTitle, request.Email, request.PhoneNumber, request.Address, request.Website, request.LinkedIn, request.GitHub, request.PhotoUrl);

        int rowsAffected = await _personalInformationEntryRepository.UpdateContentByIdAsync(updatePersonalInformationEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException("Personal information entry not found");
        }

        return new UpdatePersonalInformationEntryResponse();
    }
}