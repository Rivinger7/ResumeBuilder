using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdatePersonalInformationEntryRequest(Optional<string?> FullName, Optional<string?> ProfessionalTitle, Optional<string?> Email, Optional<string?> PhoneNumber, Optional<string?> Address, Optional<string?> Website, Optional<string?> LinkedIn, Optional<string?> GitHub, Optional<string?> PhotoUrl);
