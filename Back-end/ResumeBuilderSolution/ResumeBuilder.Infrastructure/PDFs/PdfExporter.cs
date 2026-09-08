using QuestPDF.Fluent;
using ResumeBuilder.Domain.Interfaces.Services;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Infrastructure.PDFs;

public sealed class PdfExporter : IPdfExporter
{
    public Task<byte[]> ExportAsync(ResumeInternalResponse resume, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(resume);

        ResumeDocument document = new(resume);
        byte[] pdfBytes = document.GeneratePdf();

        return Task.FromResult(pdfBytes);
    }
}
