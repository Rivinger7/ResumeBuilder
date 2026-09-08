using MediatR;
using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Interfaces.Repositories;

namespace ResumeBuilder.Application.Features.Entries.Certificates.Creates;

public sealed class CreateCertificateEntryCommandHandler(ICertificateEntryRepository certificateEntryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCertificateEntryCommand, CreateCertificateEntryResponse>
{
    private readonly ICertificateEntryRepository _certificateEntryRepository = certificateEntryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateCertificateEntryResponse> Handle(CreateCertificateEntryCommand request, CancellationToken cancellationToken)
    {
        int currentDisplayOrder = await _certificateEntryRepository.GetCurrentDisplayOrderByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken) ?? 0;

        CertificateEntry? existingEntry = await _certificateEntryRepository.GetFirstByResumeSectionIdAsync(request.ResumeSectionId, cancellationToken);

        CertificateEntry certificateEntry = new()
        {
            Id = Guid.NewGuid(),
            ResumeSectionId = request.ResumeSectionId,
            Title = request.Title,
            CertificateUrl = request.CertificateUrl,
            Description = request.Description,
            DisplayOrder = currentDisplayOrder + 1,
            CertificateLayout = existingEntry?.CertificateLayout ?? request.CertificateLayout,
            CertificateGridColumn = existingEntry?.CertificateGridColumn ?? request.CertificateGridColumn,
            CertificateRowSpacing = existingEntry?.CertificateRowSpacing ?? request.CertificateRowSpacing,
            IsStartRowsWithBullet = existingEntry?.IsStartRowsWithBullet ?? request.IsStartRowsWithBullet,
            SubinfoStyle = existingEntry?.SubinfoStyle ?? request.SubinfoStyle
        };

        await _certificateEntryRepository.AddAsync(certificateEntry, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateCertificateEntryResponse();
    }
}