using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.Languages.Updates;

public sealed record UpdateLanguageEntryCommand(Guid Id, Optional<string?> LanguageName, Optional<string?> Proficiency) : IRequest<UpdateLanguageEntryResponse>;
