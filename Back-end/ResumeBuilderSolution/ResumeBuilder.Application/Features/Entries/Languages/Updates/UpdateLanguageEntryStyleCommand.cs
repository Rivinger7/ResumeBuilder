using MediatR;
using ResumeBuilder.Domain.Enums;

namespace ResumeBuilder.Application.Features.Entries.Languages.Updates;

public sealed record UpdateLanguageEntryStyleCommand(Guid Id, LayoutType? LanguageLayout, int? LanguageGridColumn, string? LanguageRowSpacing, bool? IsStartRowsWithBullet, string? SubinfoStyle) : IRequest<UpdateLanguageEntryStyleResponse>;
