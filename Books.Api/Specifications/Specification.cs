using System.Linq.Expressions;
using Shared.Data.Enums;

namespace Books.Api.Specifications;

public class Specification<TEntity> 
    where TEntity : class
{
    public Specification(Expression<Func<TEntity, bool>>? criteria = null)
    {
        Criteria = criteria;
    }
    
    public Expression<Func<TEntity, bool>>? Criteria { get; }
    public List<Expression<Func<TEntity, object>>>? Includes { get; } = new List<Expression<Func<TEntity, object>>>();
    public (Expression<Func<TEntity, object>> Expression, OrderType Type)? OrderBy { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) 
        => Includes?.Add(includeExpression);
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) 
        => OrderBy = (orderByExpression, OrderType.Asc);
    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByExpression) 
        => OrderBy = (orderByExpression, OrderType.Desc);
}