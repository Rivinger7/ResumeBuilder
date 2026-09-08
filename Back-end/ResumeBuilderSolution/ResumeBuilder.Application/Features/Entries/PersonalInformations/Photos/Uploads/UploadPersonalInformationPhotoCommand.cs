using MediatR;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Photos.Uploads;

public sealed record UploadPersonalInformationPhotoCommand(Guid Id, Stream Content, string FileExtension) : IRequest<UploadPersonalInformationPhotoResponse>;
