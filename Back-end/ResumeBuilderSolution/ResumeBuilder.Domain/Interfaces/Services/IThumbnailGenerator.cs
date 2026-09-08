using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Domain.Interfaces.Services;

public interface IThumbnailGenerator
{
    Task<string> GenerateAsync(ResumeInternalResponse resume, CancellationToken ct = default);
    Task<byte[]?> GetAsync(Guid resumeId, CancellationToken ct = default);
}
