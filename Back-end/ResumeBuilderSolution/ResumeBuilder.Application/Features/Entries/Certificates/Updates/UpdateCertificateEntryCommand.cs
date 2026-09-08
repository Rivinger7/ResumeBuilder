using MediatR;
using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Updates;

public sealed record UpdateCertificateEntryCommand(Guid Id, Optional<string?> Title, Optional<string?> CertificateUrl, Optional<string?> Description) : IRequest<UpdateCertificateEntryResponse>;