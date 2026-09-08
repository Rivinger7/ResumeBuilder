using MediatR;

namespace ResumeBuilder.Application.Features.Resumes.Pdfs.Exports;

public sealed record ExportResumePdfQuery(Guid ResumeId, Guid UserId) : IRequest<ExportResumePdfResponse>;
