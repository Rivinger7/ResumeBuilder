using MediatR;
using ResumeBuilder.Domain.Exceptions;
using ResumeBuilder.Domain.Interfaces.Repositories;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Updates;

public sealed class UpdateCertificateEntryStyleCommandHandler(ICertificateEntryRepository certificateEntryRepository) : IRequestHandler<UpdateCertificateEntryStyleCommand, UpdateCertificateEntryStyleResponse>
{
    private readonly ICertificateEntryRepository _certificateEntryRepository = certificateEntryRepository;

    public async Task<UpdateCertificateEntryStyleResponse> Handle(UpdateCertificateEntryStyleCommand request, CancellationToken cancellationToken)
    {
        UpdateCertificateEntryStyleInternalRequest updateCertificateEntryStyleInternalRequest = new(request.Id, request.CertificateLayout, request.CertificateGridColumn, request.CertificateRowSpacing, request.IsStartRowsWithBullet, request.SubinfoStyle);

        int rowsAffected = await _certificateEntryRepository.UpdateStyleByIdAsync(updateCertificateEntryStyleInternalRequest, cancellationToken);
        if (rowsAffected == 0)
        {
            throw new NotFoundException($"No certificate entry found with Id: {request.Id}");
        }

        return new UpdateCertificateEntryStyleResponse();
    }
}