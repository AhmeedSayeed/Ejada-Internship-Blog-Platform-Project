using System.Linq.Expressions;
using Application.Interfaces;

namespace Infrastructure.Specifications;

// Inherit from this to describe one specific complex query as its own class,
// e.g. `ActiveProductsWithCategorySpec : BaseSpecification<Product>`.
public abstract class BaseSpecification<T> : ISpecification<T>
{
    protected BaseSpecification() { }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();

    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public Expression<Func<T, object>>? GroupBy { get; private set; }

    public int Skip { get; private set; }
    public int Take { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    // Read-only by default since specs are mostly used for querying, not updating.
    public bool AsNoTracking { get; private set; } = true;
    public bool AsSplitQuery { get; private set; }
    public bool IgnoreQueryFilters { get; private set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria) => Criteria = criteria;

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
        => Includes.Add(includeExpression);

    protected void AddInclude(string includeString)
        => IncludeStrings.Add(includeString);

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;

    protected void ApplyOrderByDescending(Expression<Func<T, object>> expr) => OrderByDescending = expr;

    protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression) => GroupBy = groupByExpression;

    // Call when you'll modify the entities this spec returns (e.g. load-then-update flows).
    protected void ApplyTracking() => AsNoTracking = false;

    protected void ApplySplitQuery() => AsSplitQuery = true;

    protected void ApplyIgnoreQueryFilters() => IgnoreQueryFilters = true;
}