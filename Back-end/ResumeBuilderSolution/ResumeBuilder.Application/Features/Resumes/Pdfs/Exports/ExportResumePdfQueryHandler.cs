using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Interfaces.Services;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Application.Features.Resumes.Pdfs.Exports;

public sealed class ExportResumePdfQueryHandler(IResumeRepository resumeRepository, IPdfExporter pdfExporter) : IRequestHandler<ExportResumePdfQuery, ExportResumePdfResponse>
{
    private readonly IResumeRepository _resumeRepository = resumeRepository;
    private readonly IPdfExporter _pdfExporter = pdfExporter;

    public async Task<ExportResumePdfResponse> Handle(ExportResumePdfQuery request, CancellationToken cancellationToken)
    {
        ResumeInternalResponse resume = await _resumeRepository.GetByIdWithSectionsAsync(
            request.ResumeId, request.UserId, cancellationToken) ?? throw new NotFoundException("Not found resume");

        byte[] pdfBytes = await _pdfExporter.ExportAsync(resume, cancellationToken);

        return new ExportResumePdfResponse(pdfBytes, $"{resume.Title}.pdf");
    }
}
