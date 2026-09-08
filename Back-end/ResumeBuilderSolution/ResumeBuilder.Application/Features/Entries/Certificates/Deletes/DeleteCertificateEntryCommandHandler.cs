using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Deletes;

public sealed class DeleteCertificateEntryCommandHandler(ICertificateEntryRepository certificateEntryRepository) : IRequestHandler<DeleteCertificateEntryCommand, DeleteCertificateEntryResponse>
{
    private readonly ICertificateEntryRepository _certificateEntryRepository = certificateEntryRepository;

    public async Task<DeleteCertificateEntryResponse> Handle(DeleteCertificateEntryCommand request, CancellationToken cancellationToken)
    {
        int rowsAffected = await _certificateEntryRepository.ExecuteDeleteByIdAsync(request.Id, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"Certificate entry with ID {request.Id} not found.");
        }

        return new DeleteCertificateEntryResponse();
    }
}