
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
        var books = _context.Books.ToList();
        return Ok(books);
    }
}