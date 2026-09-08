using MediatR;
using ResumeBuilder.Domain.Interfaces.Services;

namespace ResumeBuilder.Application.Features.Thumbnails.Gets;

public sealed class GetResumeThumbnailQueryHandler(IThumbnailGenerator thumbnailGenerator) : IRequestHandler<GetResumeThumbnailQuery, byte[]?>
{
    private readonly IThumbnailGenerator _thumbnailGenerator = thumbnailGenerator;

    public Task<byte[]?> Handle(GetResumeThumbnailQuery request, CancellationToken cancellationToken)
    {
        return _thumbnailGenerator.GetAsync(request.ResumeId, cancellationToken);
    }
}
