using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateLanguageEntryStyleRequest(LayoutType? LanguageLayout, int? LanguageGridColumn, string? LanguageRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle);
