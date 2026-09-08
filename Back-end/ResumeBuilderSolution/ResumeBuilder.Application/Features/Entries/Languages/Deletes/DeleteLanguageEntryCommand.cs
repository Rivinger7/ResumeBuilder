using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Languages.Deletes;

public sealed record DeleteLanguageEntryCommand(Guid Id) : IRequest<DeleteLanguageEntryResponse>;