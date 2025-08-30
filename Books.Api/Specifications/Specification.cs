using System.Linq.Expressions;

namespace Books.Api.Specifications;

public class Specification<TEntity>(Expression<Func<TEntity, bool>> criteria) 
    where TEntity : class
{
    public Expression<Func<TEntity, bool>>? Criteria { get; set; } = criteria;
}