using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface ICertificateEntryRepository : IRepository<CertificateEntry>
{
    Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default);
    Task<CertificateEntry?> GetFirstByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default);
    Task<int> UpdateContentByIdAsync(UpdateCertificateEntryInternalRequest updateCertificateEntryInternalRequest, CancellationToken cancellationToken = default);
    Task<int> UpdateStyleByIdAsync(UpdateCertificateEntryStyleInternalRequest updateCertificateEntryStyleInternalRequest, CancellationToken cancellationToken = default);
}
