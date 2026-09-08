using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public sealed class CertificateEntry : BaseEntity, IDisplayOrderedEntry
{
    public Guid ResumeSectionId { get; set; }
    public string? Title { get; set; }
    public string? CertificateUrl { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }

    public LayoutType CertificateLayout { get; set; }
    public int? CertificateGridColumn { get; set; }
    public string? CertificateRowSpacing { get; set; }
    public bool IsStartRowsWithBullet { get; set; }
    public string? SubinfoStyle { get; set; }

    public ResumeSection ResumeSection { get; set; } = null!;
}
