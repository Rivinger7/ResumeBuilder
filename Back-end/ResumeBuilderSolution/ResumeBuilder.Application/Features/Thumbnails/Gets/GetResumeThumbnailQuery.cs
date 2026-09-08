using MediatR;

namespace ResumeBuilder.Application.Features.Thumbnails.Gets;

public sealed record GetResumeThumbnailQuery(Guid ResumeId) : IRequest<byte[]?>;
