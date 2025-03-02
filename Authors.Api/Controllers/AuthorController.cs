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
        var authors = _context.Authors.ToList();
        return Ok(authors);
    }
}