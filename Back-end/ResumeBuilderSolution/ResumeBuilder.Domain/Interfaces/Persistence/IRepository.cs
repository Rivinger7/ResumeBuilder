using ResumeBuilder.Domain.Common;

namespace ResumeBuilder.Domain.Interfaces.Persistence;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<int> ExecuteDeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
