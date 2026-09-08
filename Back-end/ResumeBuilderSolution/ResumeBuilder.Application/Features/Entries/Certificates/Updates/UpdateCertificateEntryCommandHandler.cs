using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Updates;

public sealed class UpdateCertificateEntryCommandHandler(ICertificateEntryRepository certificateEntryRepository) : IRequestHandler<UpdateCertificateEntryCommand, UpdateCertificateEntryResponse>
{
    private readonly ICertificateEntryRepository _certificateEntryRepository = certificateEntryRepository;

    public async Task<UpdateCertificateEntryResponse> Handle(UpdateCertificateEntryCommand request, CancellationToken cancellationToken)
    {
        UpdateCertificateEntryInternalRequest updateCertificateEntryInternalRequest = new(request.Id, request.Title, request.CertificateUrl, request.Description);

        int rowsAffected = await _certificateEntryRepository.UpdateContentByIdAsync(updateCertificateEntryInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"No certificate entry found with Id: {request.Id}");
        }

        return new UpdateCertificateEntryResponse();
    }
}