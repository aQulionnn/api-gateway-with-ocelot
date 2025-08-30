namespace Books.Api.Specifications;

public static class SpecificationQueryBuilder
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> query, Specification<TEntity> specification) where TEntity : class
    {
        var queryResult = query;
        
        if (specification.Criteria is not null)
            queryResult = queryResult.Where(specification.Criteria);
        
        return queryResult;
    }
}