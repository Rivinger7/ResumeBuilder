using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Domain.Interfaces.Services;

public interface IPdfExporter
{
    Task<byte[]> ExportAsync(ResumeInternalResponse resume, CancellationToken ct = default);
}
