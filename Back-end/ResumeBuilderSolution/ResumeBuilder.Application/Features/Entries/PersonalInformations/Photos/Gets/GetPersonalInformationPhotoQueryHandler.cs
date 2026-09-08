using MediatR;
using ResumeBuilder.Domain.Interfaces.Services;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Photos.Gets;

public sealed class GetPersonalInformationPhotoQueryHandler(IPhotoStorage photoStorage) : IRequestHandler<GetPersonalInformationPhotoQuery, (byte[] Content, string ContentType)?>
{
    private readonly IPhotoStorage _photoStorage = photoStorage;

    public Task<(byte[] Content, string ContentType)?> Handle(GetPersonalInformationPhotoQuery request, CancellationToken cancellationToken)
    {
        return _photoStorage.GetAsync(request.Id, cancellationToken);
    }
}
