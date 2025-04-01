using Microsoft.AspNetCore.Mvc;
using Shared.Data.Data;

namespace Books.Api.Controllers;

[Route("api/books")]
[ApiController]
public class BookController(IAppDbContext context) : ControllerBase
{
    private readonly IAppDbContext _context = context;

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
        var books = _context.Books.Where(b => b.AuthorId.Value == id)
            .Select(x => new
            {
                x.Id, 
                x.Title,
                x.Format.Name
            }).ToList();
        
        return Ok(books);
    }
}