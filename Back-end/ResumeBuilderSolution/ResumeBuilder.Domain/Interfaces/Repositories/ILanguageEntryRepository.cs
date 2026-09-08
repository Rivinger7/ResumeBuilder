using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface ILanguageEntryRepository : IRepository<LanguageEntry>
{
    Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default);
    Task<int> UpdateContentByIdAsync(UpdateLanguageEntryInternalRequest updateLanguageEntryInternalRequest, CancellationToken cancellationToken = default);
    Task<int> UpdateStyleByIdAsync(UpdateLanguageEntryStyleInternalRequest updateLanguageEntryStyleInternalRequest, CancellationToken cancellationToken = default);
}
