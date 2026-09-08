using MediatR;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Deletes;

public sealed record DeletePersonalInformationEntryCommand(Guid Id) : IRequest<DeletePersonalInformationEntryResponse>;