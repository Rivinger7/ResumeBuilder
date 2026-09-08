using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Interfaces.Services;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Application.Features.Thumbnails.Creates;

public sealed class GeneratorResumeThumbnailCommandHandler(IResumeRepository resumeRepository, IThumbnailGenerator thumbnailGenerator) : IRequestHandler<GeneratorResumeThumbnailCommand, GeneratorResumeThumbnailResponse>
{
    private readonly IResumeRepository _resumeRepository = resumeRepository;
    private readonly IThumbnailGenerator _thumbnailGenerator = thumbnailGenerator;

    public async Task<GeneratorResumeThumbnailResponse> Handle(GeneratorResumeThumbnailCommand request, CancellationToken cancellationToken)
    {
        ResumeInternalResponse resumeInternalResponse = await _resumeRepository.GetByIdWithSectionsAsync(
            request.ResumeId, request.UserId, cancellationToken) ?? throw new NotFoundException("Not found resume");
        string thumbnailUrl = await _thumbnailGenerator.GenerateAsync(resumeInternalResponse, cancellationToken);

        // ExecuteUpdateAsync ngay trong repository — không cần load entity gốc / SaveChanges thủ công,
        // đúng pattern DisplayOrderService đang dùng trong project.
        await _resumeRepository.UpdateThumbnailUrlAsync(request.ResumeId, thumbnailUrl, cancellationToken);

        return new GeneratorResumeThumbnailResponse(thumbnailUrl);
    }
}