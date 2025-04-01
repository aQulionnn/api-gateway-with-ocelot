using Microsoft.EntityFrameworkCore;
using Shared.Data.Common;
using Shared.Data.Entities;
using Shared.Data.Enums;

namespace Shared.Data.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        SeedData();
    }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => await base.SaveChangesAsync(cancellationToken);

    private void SeedData()
    {
        if (!Authors.Any())
            Authors.Add(new Author { Id = new AuthorId(1), Name = "Frank Herbert" });
        
        if (!Books.Any())
            Books.Add(new Book { Id = new BookId(1), Title = "Dune", Format = FormatType.EBook, AuthorId = new AuthorId(1) });
    
        SaveChanges();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<FormatType>();

        modelBuilder.Entity<Book>()
            .Property(b => b.FormatValue);
        
        modelBuilder.Entity<Book>()
            .Property(b => b.Id)
            .HasConversion(id => id.Value, id => new BookId(id));
        
        modelBuilder.Entity<Book>()
            .Property(b => b.AuthorId)
            .HasConversion(id => id.Value, id => new AuthorId(id));
        
        modelBuilder.Entity<Author>()
            .Property(a => a.Id)
            .HasConversion(id => id.Value, value => new AuthorId(value));
    }
}