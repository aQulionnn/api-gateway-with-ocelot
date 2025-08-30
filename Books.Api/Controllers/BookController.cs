using Books.Api.Repositories;
using Books.Api.Specifications;
using Microsoft.AspNetCore.Mvc;
using Shared.Data.Data;

namespace Books.Api.Controllers;

[Route("api/books")]
[ApiController]
public class BookController(IAppDbContext context, IBookRepository bookRepository) 
    : ControllerBase
{
    private readonly IAppDbContext _context = context;
    private readonly IBookRepository _bookRepository = bookRepository;
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var books = _context.Books
            .Select(b => new
            {
                b.Id,
                b.Title,
                b.Format.Name,
                b.AuthorId
            }).ToList();
        
        return Ok(books);
    }

    [HttpGet]
    [Route("author/{id:int}")]
    public IActionResult GetByAuthorId([FromRoute] int id)
    {
        var specification = new AuthorIdSpecification(id);
        var books = _bookRepository
            .GetBooksByAuthorId(id, specification)
            .Select(x => new
            {
                x.Id, 
                x.Title,
                x.Format.Name
            });
        
        return Ok(books);
    }
}