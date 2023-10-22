using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web.Common.Data.DataBase;

public static class EnsureMigration
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationContext>>();
            using var context = factory.CreateDbContext();
            context.Database.Migrate();
        }

        return host;
    }
}