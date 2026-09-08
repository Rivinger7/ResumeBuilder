using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Enums;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface IResumeSectionRepository : IRepository<ResumeSection>
{
    Task<bool> IsResumeSectionExistedByType(Guid resumeId, ResumeSectionType type, CancellationToken cancellationToken = default);
}
