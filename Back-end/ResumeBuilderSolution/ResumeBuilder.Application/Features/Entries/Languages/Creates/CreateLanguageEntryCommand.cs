using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Languages.Creates;

public sealed record CreateLanguageEntryCommand(Guid ResumeSectionId, string? LanguageName, string? Proficiency, LayoutType LanguageLayout, int? LanguageGridColumn, string? LanguageRowSpacing, bool IsStartRowsWithBullet, string? SubinfoStyle) : IRequest<CreateLanguageEntryResponse>;