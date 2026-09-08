using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Creates;

public sealed record CreateCertificateEntryCommand(Guid ResumeSectionId, string? Title, string? CertificateUrl, string? Description, LayoutType CertificateLayout, int? CertificateGridColumn, string? CertificateRowSpacing, bool IsStartRowsWithBullet, string? SubinfoStyle) : IRequest<CreateCertificateEntryResponse>;