using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Updates;

public sealed class UpdatePersonalInformationEntryStyleCommandHandler(IPersonalInformationEntryRepository personalInformationEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonalInformationEntryStyleCommand, UpdatePersonalInformationEntryStyleResponse>
{
    private readonly IPersonalInformationEntryRepository _personalInformationEntryRepository = personalInformationEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdatePersonalInformationEntryStyleResponse> Handle(UpdatePersonalInformationEntryStyleCommand request, CancellationToken cancellationToken)
    {
        UpdatePersonalInformationEntryStyleInternalRequest updatePersonalInformationEntryStyleInternalRequest = new(request.Id, request.PersonalInformationTextAlignment, request.PersonalInformationLayout, request.PersonalInformationMarkerStyle, request.PersonalInformationIconStyle, request.IsTitleProfessionalTitleOnSameLine);

        int rowsAffected = await _personalInformationEntryRepository.UpdateStyleByIdAsync(updatePersonalInformationEntryStyleInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"No personal information entry found with Id: {request.Id}");
        }

        return new UpdatePersonalInformationEntryStyleResponse();
    }
}