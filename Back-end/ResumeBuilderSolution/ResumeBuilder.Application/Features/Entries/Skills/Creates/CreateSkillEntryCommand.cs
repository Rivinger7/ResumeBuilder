using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Skills.Creates;

public sealed record CreateSkillEntryCommand(Guid ResumeSectionId, string? SkillName, string? Description, string? SkillLevel, LayoutType SkillLayout, int? SkillGridColumn, string? SkillRowSpacing, bool IsStartRowsWithBullet, string? SubinfoStyle) : IRequest<CreateSkillEntryResponse>;
