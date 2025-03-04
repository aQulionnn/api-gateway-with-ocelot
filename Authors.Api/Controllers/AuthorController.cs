using Microsoft.AspNetCore.Mvc;
using Shared.Data;
using Shared.Data.Entities;

namespace Authors.Api.Controllers;

[Route("api/authors")]
[ApiController]
public class AuthorController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public ActionResult GetAll()
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
    public ActionResult GetById([FromRoute] int id)
    {
        var author = _context.Authors
            .Where(a => a.Id == id)
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