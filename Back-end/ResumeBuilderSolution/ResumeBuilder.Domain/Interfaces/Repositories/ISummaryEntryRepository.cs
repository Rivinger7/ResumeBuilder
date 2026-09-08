using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface ISummaryEntryRepository : IRepository<SummaryEntry>
{
    Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default);
    Task<int> UpdateContentByIdAsync(UpdateSummaryEntryInternalRequest updateSummaryEntryInternalRequest, CancellationToken cancellationToken = default);
}
