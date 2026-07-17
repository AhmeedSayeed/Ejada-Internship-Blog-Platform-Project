using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

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

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
        => await DbSet.FindAsync(new object?[] { id }, ct);

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = DbSet;
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        var keyProperty = Context.Model.FindEntityType(typeof(TEntity))!
            .FindPrimaryKey()!.Properties[0].Name;

        return await query.FirstOrDefaultAsync(e => Equals(EF.Property<TKey>(e, keyProperty), id));
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
        => await DbSet.AsNoTracking().ToListAsync(ct);

    public virtual async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => await DbSet.AsNoTracking().Where(predicate).ToListAsync(ct);

    public virtual async Task<TEntity?> SingleOrDefaultAsync(ISpecification<TEntity> spec, CancellationToken ct = default)
        => await ApplySpec(spec).FirstOrDefaultAsync(ct);

    public virtual async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken ct = default)
        => await ApplySpec(spec).ToListAsync(ct);

    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(
        int pageIndex, int pageSize, ISpecification<TEntity>? spec = null, CancellationToken ct = default)
    {
        var countQuery = spec is null ? DbSet.AsQueryable() : ApplySpec(spec, applyPaging: false);
        var totalCount = await countQuery.CountAsync(ct);

        var itemsQuery = spec is null ? DbSet.AsQueryable() : ApplySpec(spec, applyPaging: false);
        var items = await itemsQuery
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

        if (entity is ISoftDelete softDeletable)
        {
            softDeletable.IsDeleted = true;
            softDeletable.DeletedAt = DateTime.UtcNow;
            Update(entity);
        }
        else
        {
            Remove(entity);
        }

        return true;
    }

    private IQueryable<TEntity> ApplySpec(ISpecification<TEntity> spec, bool applyPaging = true)
        => SpecificationEvaluator<TEntity>.GetQuery(DbSet.AsQueryable(), spec, applyPaging);
}
