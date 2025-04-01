using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

namespace Shared.Data.Data;

public interface IAppDbContext
{
    DbSet<Author> Authors { get; set; }
    DbSet<Book> Books { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}