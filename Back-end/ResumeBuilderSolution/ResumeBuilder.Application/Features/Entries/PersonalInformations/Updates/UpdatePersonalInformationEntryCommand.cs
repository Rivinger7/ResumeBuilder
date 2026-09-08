using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.PersonalInformations.Updates;

public sealed record UpdatePersonalInformationEntryCommand(Guid Id, Optional<string?> FullName, Optional<string?> ProfessionalTitle, Optional<string?> Email, Optional<string?> PhoneNumber, Optional<string?> Address, Optional<string?> Website, Optional<string?> LinkedIn, Optional<string?> GitHub, Optional<string?> PhotoUrl) : IRequest<UpdatePersonalInformationEntryResponse>;
