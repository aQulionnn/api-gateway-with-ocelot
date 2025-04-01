using Microsoft.EntityFrameworkCore;

namespace Shared.Data.Data;

public class AppDbContextFactory(IDbContextFactory<AppDbContext> factory) : IDbContextFactory<AppDbContext>
{
    private readonly IDbContextFactory<AppDbContext> _factory = factory;
    
    public AppDbContext CreateDbContext() => _factory.CreateDbContext();
}