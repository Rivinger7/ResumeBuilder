using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public record UpdateCertificateEntryStyleInternalRequest(Guid Id, LayoutType? CertificateLayout, int? CertificateGridColumn, string? CertificateRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle);