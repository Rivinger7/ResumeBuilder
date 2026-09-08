using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Entities.Resumes;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Domain.Models.Resumes;

namespace ResumeBuilder.Domain.Interfaces.Repositories;

public interface IResumeRepository : IRepository<Resume>
{
    Task<ResumeInternalResponse?> GetByIdWithSectionsAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<ResumeInternalResponse>> GetPagedByUserIdAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> IsResumeExistedByTitle(string title, Guid userId, CancellationToken cancellationToken = default);
    Task UpdateThumbnailUrlAsync(Guid id, string thumbnailUrl, CancellationToken cancellationToken = default);
}
