using ResumeBuilder.Domain.Entities.Resumes.Entries;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Resumes.Entries;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface ISkillEntryRepository : IRepository<SkillEntry>
{
    Task<int?> GetCurrentDisplayOrderByResumeSectionIdAsync(Guid resumeSectionId, CancellationToken cancellationToken = default);
    Task<int> UpdateContentByIdAsync(UpdateSkillEntryInternalRequest updateSkillEntryInternalRequest, CancellationToken cancellationToken = default);
    Task<int> UpdateStyleByIdAsync(UpdateSkillEntryStyleInternalRequest updateSkillEntryStyleInternalRequest, CancellationToken cancellationToken = default);
}
