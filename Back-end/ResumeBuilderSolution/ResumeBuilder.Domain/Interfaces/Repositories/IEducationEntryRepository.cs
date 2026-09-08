using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface IEducationEntryRepository : IRepository<EducationEntry>
{
    Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default);
    Task<int> UpdateContentByIdAsync(UpdateEducationEntryInternalRequest updateEducationEntryInternalRequest, CancellationToken cancellationToken = default);
}
