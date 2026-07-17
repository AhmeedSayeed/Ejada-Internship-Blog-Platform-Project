namespace Application.Interfaces;

// Only needed once a use case touches more than one repository and they must
// succeed or fail together (e.g. checkout: Orders + Inventory + Payments).
// Small CRUD APIs can skip this and inject IRepository<T, TKey> directly.
public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
