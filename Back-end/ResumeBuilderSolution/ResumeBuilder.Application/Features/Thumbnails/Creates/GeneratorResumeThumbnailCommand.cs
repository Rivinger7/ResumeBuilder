using MediatR;

namespace ResumeBuilder.Application.Features.Thumbnails.Creates;

public sealed record GeneratorResumeThumbnailCommand(Guid ResumeId, Guid UserId) : IRequest<GeneratorResumeThumbnailResponse>;