using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

namespace Shared.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        SeedData();
    }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    private void SeedData()
    {
        if (!Authors.Any())
            Authors.Add(new Author { Id = 1, Name = "Frank Herbert" });
        
        if (!Books.Any())
            Books.Add(new Book { Id = 1, Title = "Dune", AuthorId = 1 });
    
        SaveChanges();
    }
}