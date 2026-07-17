using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Specifications;

public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec, bool applyPaging = true)
    {
        var query = inputQuery;

        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        if (spec.IgnoreQueryFilters)
            query = query.IgnoreQueryFilters();

        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
        query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.GroupBy is not null)
            query = query.GroupBy(spec.GroupBy).SelectMany(g => g);

        if (applyPaging && spec.IsPagingEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);

        if (spec.AsSplitQuery)
            query = query.AsSplitQuery();

        if (spec.AsNoTracking)
            query = query.AsNoTracking();

        return query;
    }
}
