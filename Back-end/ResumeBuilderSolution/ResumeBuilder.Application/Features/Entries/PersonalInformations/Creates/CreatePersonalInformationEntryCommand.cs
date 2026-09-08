using MediatR;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Creates;

public sealed record CreatePersonalInformationEntryCommand() : IRequest<CreatePersonalInformationEntryResponse>;