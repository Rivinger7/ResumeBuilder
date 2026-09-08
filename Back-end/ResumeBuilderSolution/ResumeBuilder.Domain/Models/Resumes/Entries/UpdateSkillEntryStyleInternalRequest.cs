using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateSkillEntryStyleInternalRequest(Guid Id, LayoutType? SkillLayout, int? SkillGridColumn, string? SkillRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle);
