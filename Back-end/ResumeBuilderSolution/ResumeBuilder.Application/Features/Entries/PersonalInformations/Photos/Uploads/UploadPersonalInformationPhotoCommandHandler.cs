using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Interfaces.Services;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Photos.Uploads;

public sealed class UploadPersonalInformationPhotoCommandHandler(IPersonalInformationEntryRepository personalInformationEntryRepository, IPhotoStorage photoStorage, IUnitOfWork unitOfWork) : IRequestHandler<UploadPersonalInformationPhotoCommand, UploadPersonalInformationPhotoResponse>
{
    private readonly IPersonalInformationEntryRepository _personalInformationEntryRepository = personalInformationEntryRepository;
    private readonly IPhotoStorage _photoStorage = photoStorage;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UploadPersonalInformationPhotoResponse> Handle(UploadPersonalInformationPhotoCommand request, CancellationToken cancellationToken)
    {
        PersonalInformationEntry? entry = await _personalInformationEntryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entry is null)
        {
            throw new NotFoundException($"Personal information entry with ID {request.Id} not found.");
        }

        string photoUrl = await _photoStorage.SaveAsync(request.Id, request.Content, request.FileExtension, cancellationToken);

        entry.PhotoUrl = photoUrl;
        _personalInformationEntryRepository.Update(entry);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UploadPersonalInformationPhotoResponse(photoUrl);
    }
}
