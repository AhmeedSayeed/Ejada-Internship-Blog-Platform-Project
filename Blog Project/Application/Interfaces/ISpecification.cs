using System.Linq.Expressions;

namespace Application.Interfaces;

// Describes a query (filter + includes + sorting + paging) without leaking
// raw LINQ/EF Core into services or controllers. Only needed once a query
// gets complex - simple lookups can just use IRepository's plain methods.
public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }

    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    Expression<Func<T, object>>? GroupBy { get; }

    int Skip { get; }
    int Take { get; }
    bool IsPagingEnabled { get; }

    bool AsNoTracking { get; }
    bool AsSplitQuery { get; }
    bool IgnoreQueryFilters { get; }
}
