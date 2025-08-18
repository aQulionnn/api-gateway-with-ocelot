using Authors.Api.Contracts;
using Authors.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared.Data.Data;
using Shared.Data.Entities;

namespace Authors.Api.Controllers;

[Route("api/authors")]
[ApiController]
public class AuthorController(IAppDbContext context, IMemoryCache cache, IServiceScopeFactory scopeFactory) 
    : ControllerBase
{
    private readonly IAppDbContext _context = context;
    private readonly IMemoryCache _cache = cache;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

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
        
        var etag = ETag.Generate(author);
        
        var requestETag = Request.Headers["If-None-Match"].FirstOrDefault();
        if (requestETag == etag)
            return StatusCode(StatusCodes.Status304NotModified);
        
        Response.Headers.ETag = etag;
        
        return Ok(author);
    }

    [HttpPut]
    [Route("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateAuthorRequest request)
    {
        string key = $"authors:{id}";
        var author = new AuthorResponse(id, request.Name);
        _cache.Set(key, author, TimeSpan.FromHours(1));

        Task.Run(async () =>
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var entity = await context.Authors.FindAsync(id);

            if (entity is not null)
            {
                entity.Name = request.Name;
                await context.SaveChangesAsync();
            }
        });
        
        return Ok(author);
    }
}