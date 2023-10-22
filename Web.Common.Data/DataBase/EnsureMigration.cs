using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Common.Data.DataBase;

public static class EnsureMigration
{
    public static void EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : IDbContextFactory<ApplicationContext>
    {
        var contextFactory = app.ApplicationServices.GetService<T>();
        using var context = contextFactory.CreateDbContext();
        context.Database.Migrate();
    }
}