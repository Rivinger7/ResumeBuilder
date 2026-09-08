using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface IObjectiveEntryRepository : IRepository<ObjectiveEntry>
{
    Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default);
    Task<int> UpdateContentByIdAsync(UpdateObjectiveEntryInternalRequest updateObjectiveEntryInternalRequest, CancellationToken cancellationToken = default);
}
