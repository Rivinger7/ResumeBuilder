using MediatR;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Photos.Gets;

public sealed record GetPersonalInformationPhotoQuery(Guid Id) : IRequest<(byte[] Content, string ContentType)?>;
