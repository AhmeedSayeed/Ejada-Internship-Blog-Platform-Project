using System.Linq.Expressions;
using Infrastructure.Repository;

namespace Application.Interfaces;

// TEntity  = the entity type (Product, Order, ...)
// TKey     = the primary key type (int, Guid, ...)
//
// A small project only ever calls the plain Get/Find/Add/Update/Remove methods.
// A larger project starts using ISpecification-based methods once queries need
// filtering + includes + sorting + paging together.
public interface IRepository<TEntity, TKey> where TEntity : class
{
    // ---------- Reads ----------
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes);
    Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> SingleOrDefaultAsync(ISpecification<TEntity> spec, CancellationToken ct = default);
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken ct = default);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes);

    Task<IReadOnlyList<TEntity>> GetByIdsAsync(
        IEnumerable<TKey> ids, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes);

    IQueryable<TEntity> Query(bool asNoTracking = true);

    Task<PagedResult<TEntity>> GetPagedAsync(
        int pageIndex, int pageSize, ISpecification<TEntity>? spec = null, CancellationToken ct = default);

    Task<int> CountAsync(ISpecification<TEntity>? spec = null, CancellationToken ct = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

    // ---------- Writes ----------
    Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    // Soft-deletes automatically if the entity implements ISoftDelete, otherwise removes it.
    Task<bool> DeleteByIdAsync(TKey id, CancellationToken ct = default);
    Task<int> CountAsync(
    Expression<Func<TEntity, bool>> predicate,
    CancellationToken ct = default);
}