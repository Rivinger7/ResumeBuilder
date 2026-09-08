using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateCertificateEntryStyleRequest(LayoutType? CertificateLayout, int? CertificateGridColumn, string? CertificateRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle);
