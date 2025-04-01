using Microsoft.AspNetCore.Mvc;
using Shared.Data.Data;

namespace Authors.Api.Controllers;

[Route("api/authors")]
[ApiController]
public class AuthorController(IAppDbContext context) : ControllerBase
{
    private readonly IAppDbContext _context = context;

    [HttpGet]
    public IActionResult GetAll()
    {
        var authors = _context.Authors
            .Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();
        
        return Ok(authors);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var author = _context.Authors
            .Where(a => a.Id.Value == id)
            .Select(a => new
            {
                Id = a.Id, 
                Name = a.Name
            })
            .SingleOrDefault();
        
        if (author == null)
            return NotFound();
        
        return Ok(author);
    }
}