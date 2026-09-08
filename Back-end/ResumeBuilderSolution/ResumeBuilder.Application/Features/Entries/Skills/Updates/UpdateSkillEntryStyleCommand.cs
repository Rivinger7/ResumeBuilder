using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Skills.Updates;

public sealed record UpdateSkillEntryStyleCommand(Guid Id, LayoutType? SkillLayout, int? SkillGridColumn, string? SkillRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle) : IRequest<UpdateSkillEntryStyleResponse>;
