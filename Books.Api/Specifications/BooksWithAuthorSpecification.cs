using System.Linq.Expressions;
using Shared.Data.Entities;

namespace Books.Api.Specifications;

public class BooksWithAuthorSpecification : Specification<Book>
{
    public BooksWithAuthorSpecification() : base()
    {
        AddInclude(book => book.Author);
    }
}