using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateSkillEntryStyleRequest(LayoutType? SkillLayout, int? SkillGridColumn, string? SkillRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle);
