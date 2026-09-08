using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Api.Models.Entries;

public sealed record UpdateCertificateEntryRequest(Optional<string?> Title, Optional<string?> CertificateUrl, Optional<string?> Description);
