using MediatR;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Deletes;

public sealed record DeleteCertificateEntryCommand(Guid Id) : IRequest<DeleteCertificateEntryResponse>;