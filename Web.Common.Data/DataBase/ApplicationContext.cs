using Microsoft.EntityFrameworkCore;
using Web.Common.Entity.Entity;
using WebUtilities.Interfaces;

namespace Web.Common.Data.DataBase;

public class ApplicationContext: DbContext, IContext
{
    public DbSet<DemoObject> DemoObjects { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
}