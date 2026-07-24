using Application.Interfaces;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using System.Linq.Expressions;

namespace Infrastructure.Repository;

// Assumes a single-column primary key (adjust GetByIdAsync(id, includes) if you
// use composite keys - EF's plain FindAsync already handles composite keys fine).
public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes)
    {
        if (includes.Length == 0)
            return await DbSet.FindAsync(new object?[] { id }, ct);

        IQueryable<TEntity> query = DbSet;
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        var keyProperty = Context.Model.FindEntityType(typeof(TEntity))!
            .FindPrimaryKey()!.Properties[0].Name;

        return await query.FirstOrDefaultAsync(e => Equals(EF.Property<TKey>(e, keyProperty), id), ct);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.ToListAsync(ct);
    }

    public virtual async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking().Where(predicate);
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.ToListAsync(ct);
    }

    public virtual async Task<TEntity?> SingleOrDefaultAsync(ISpecification<TEntity> spec, CancellationToken ct = default)
        => await ApplySpec(spec).SingleOrDefaultAsync(ct);

    public virtual async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken ct = default)
        => await ApplySpec(spec).ToListAsync(ct);

    public virtual async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = DbSet.Where(predicate);
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.FirstOrDefaultAsync(ct);
    }

    public virtual async Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = DbSet.Where(predicate);
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.SingleOrDefaultAsync(ct);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetByIdsAsync(
        IEnumerable<TKey> ids, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes)
    {
        var idList = ids as ICollection<TKey> ?? ids.ToList();
        var keyProperty = Context.Model.FindEntityType(typeof(TEntity))!
            .FindPrimaryKey()!.Properties[0].Name;

        IQueryable<TEntity> query = DbSet.AsNoTracking()
            .Where(e => idList.Contains(EF.Property<TKey>(e, keyProperty)));

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.ToListAsync(ct);
    }

    public virtual IQueryable<TEntity> Query(bool asNoTracking = true)
        => asNoTracking ? DbSet.AsNoTracking() : DbSet;

    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(
        int pageIndex, int pageSize, ISpecification<TEntity>? spec = null, CancellationToken ct = default)
    {
        var query = spec is null ? DbSet.AsQueryable() : ApplySpec(spec, applyPaging: false);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<TEntity>(items, totalCount, pageIndex, pageSize);
    }

    public virtual async Task<int> CountAsync(ISpecification<TEntity>? spec = null, CancellationToken ct = default)
        => spec is null
            ? await DbSet.CountAsync(ct)
            : await ApplySpec(spec, applyPaging: false).CountAsync(ct);

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => await DbSet.AnyAsync(predicate, ct);

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        await DbSet.AddAsync(entity, ct);
        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        => await DbSet.AddRangeAsync(entities, ct);

    public virtual void Update(TEntity entity)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities) => DbSet.UpdateRange(entities);

    public virtual void Remove(TEntity entity) => DbSet.Remove(entity);

    public virtual void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);

    public virtual async Task<bool> DeleteByIdAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(id, ct);
        if (entity is null) return false;

        Remove(entity);

        return true;
    }

    private IQueryable<TEntity> ApplySpec(ISpecification<TEntity> spec, bool applyPaging = true)
        => SpecificationEvaluator<TEntity>.GetQuery(DbSet.AsQueryable(), spec, applyPaging);

    public virtual async Task<int> CountAsync(
    Expression<Func<TEntity, bool>> predicate,
    CancellationToken ct = default)
    {
        return await DbSet.CountAsync(predicate, ct);
    }
}