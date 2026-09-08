using Microsoft.EntityFrameworkCore.Storage;
using ResumeBuilder.Domain.Interfaces.Persistence;

namespace ResumeBuilder.Persistence.UnitOfWorks;

internal sealed class UnitOfWorkTransaction(IDbContextTransaction transaction) : IUnitOfWorkTransaction
{
    private readonly IDbContextTransaction _transaction = transaction;

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _transaction.CommitAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return _transaction.RollbackAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return _transaction.DisposeAsync();
    }
}
