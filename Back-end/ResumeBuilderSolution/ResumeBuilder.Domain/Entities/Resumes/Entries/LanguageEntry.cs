using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Entities.Resumes.Entries;

public sealed class LanguageEntry : BaseEntity, IDisplayOrderedEntry
{
    public Guid ResumeSectionId { get; set; }
    public string? LanguageName { get; set; }
    public string? Proficiency { get; set; }
    public int DisplayOrder { get; set; }
    public LayoutType LanguageLayout { get; set; }
    public int? LanguageGridColumn { get; set; }
    public string? LanguageRowSpacing { get; set; }
    public bool IsStartRowsWithBullet { get; set; }
    public string? SubinfoStyle { get; set; }

    // Todo: Consider adding a FileStorage entity to handle file uploads for language certificates or related documents.
    //public class FileStorage
    //{
    //    public Guid Id { get; set; }

    //    public string FileName { get; set; } = null!;
    //    public string ContentType { get; set; } = null!;
    //    public long Size { get; set; }

    //    public string BlobPath { get; set; } = null!;
    //}

    public ResumeSection ResumeSection { get; set; } = null!;
}
