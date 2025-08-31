using Microsoft.EntityFrameworkCore;
using Shared.Data.Enums;

namespace Books.Api.Specifications;

public static class SpecificationQueryBuilder
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> query, Specification<TEntity> specification) where TEntity : class
    {
        var queryResult = query;
        
        if (specification.Criteria is not null)
            queryResult = queryResult.Where(specification.Criteria);

        if (specification.Includes is not null)
            queryResult = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
        
        if (specification.OrderBy is not null)
            queryResult = specification.OrderBy.Value.Type == OrderType.Asc
                ? queryResult.OrderBy(specification.OrderBy.Value.Expression)
                : queryResult.OrderByDescending(specification.OrderBy.Value.Expression);
        
        return queryResult;
    }
}