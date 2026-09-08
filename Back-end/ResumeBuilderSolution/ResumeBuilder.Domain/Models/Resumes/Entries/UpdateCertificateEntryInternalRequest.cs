using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Models.Resumes.Entries;

public sealed record UpdateCertificateEntryInternalRequest(Guid Id, Optional<string?> Title, Optional<string?> CertificateUrl, Optional<string?> Description);
