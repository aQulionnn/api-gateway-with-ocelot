using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Data;

namespace Shared.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services)
    {
        services.AddPooledDbContextFactory<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("Database");
        });

        services.AddScoped<IAppDbContext>(options =>
            options.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());
        
        return services;
    }
}