using Books.Api.Specifications;
using Shared.Data.Data;
using Shared.Data.Entities;

namespace Books.Api.Repositories;

public class BookRepository(IAppDbContext context) : IBookRepository
{
    private readonly IAppDbContext _context = context;
    
    public IEnumerable<Book> GetBooksByAuthorId(int authorId, Specification<Book> specification)
    {
        return SpecificationQueryBuilder
            .GetQuery(_context.Books, specification)
            .ToList();
    }
}

public interface IBookRepository
{
    IEnumerable<Book> GetBooksByAuthorId(int authorId, Specification<Book> specification);
}