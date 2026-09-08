using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Updates;

public sealed record UpdateCertificateEntryStyleCommand(Guid Id, LayoutType? CertificateLayout, int? CertificateGridColumn, string? CertificateRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle) : IRequest<UpdateCertificateEntryStyleResponse>;