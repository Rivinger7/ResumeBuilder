using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdatePersonalInformationEntryInternalRequest(Guid Id, Optional<string?> FullName, Optional<string?> ProfessionalTitle, Optional<string?> Email, Optional<string?> PhoneNumber, Optional<string?> Address, Optional<string?> Website, Optional<string?> LinkedIn, Optional<string?> GitHub, Optional<string?> PhotoUrl);
