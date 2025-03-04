
using Microsoft.AspNetCore.Mvc;
using Shared.Data;

namespace Books.Api.Controllers;

[Route("/api/books")]
[ApiController]
public class BookController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public IActionResult GetAll()
    {
        var books = _context.Books
            .Select(b => new
            {
                b.Id,
                b.Title,
                b.AuthorId
            }).ToList();
        
        return Ok(books);
    }

    [HttpGet]
    [Route("author/{id:int}")]
    public IActionResult GetByAuthorId([FromRoute] int id)
    {
        var books = _context.Books.Where(b => b.AuthorId == id)
            .Select(x => new
            {
                x.Id, 
                x.Title
            }).ToList();
        
        return Ok(books);
    }
}