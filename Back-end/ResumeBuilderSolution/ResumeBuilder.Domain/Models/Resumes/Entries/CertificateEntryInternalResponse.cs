using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record CertificateEntryInternalResponse(Guid Id, string? Title, string? CertificateUrl, string? Description, int DisplayOrder, LayoutType CertificateLayout, int? CertificateGridColumn, string? CertificateRowSpacing, bool IsStartRowsWithBullet, string? SubinfoStyle);
