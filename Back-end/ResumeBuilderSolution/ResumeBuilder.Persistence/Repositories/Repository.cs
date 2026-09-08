using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Domain.Common;
using ResumeBuilder.Domain.Interfaces.Persistence;
using ResumeBuilder.Persistence.Context;

namespace ResumeBuilder.Persistence.Repositories;

public abstract class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context = context;

    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public async virtual Task<int> ExecuteDeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}
