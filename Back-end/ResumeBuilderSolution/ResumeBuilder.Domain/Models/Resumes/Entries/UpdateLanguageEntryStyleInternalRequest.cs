using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateLanguageEntryStyleInternalRequest(Guid Id, LayoutType? LanguageLayout, int? LanguageGridColumn, string? LanguageRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle);
