using Microsoft.EntityFrameworkCore;
using WebUtilities.EventBus.Events;
using WebUtilities.Interfaces;
using WebUtilities.Model;

namespace WebUtilities.Services;

public class Repository<T> : IRepository<T> where T : BaseObject
{
    public async Task<string> CreateAsync(IContext context, T entity)
    {
        var dbContext = (DbContext) context;
        if(string.IsNullOrEmpty(entity.Id))
            entity.Id = Guid.NewGuid().ToString();
        
        entity.UpdatedDateTime = DateTime.UtcNow;
        dbContext.Add(entity);

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteAsync(IContext context, T entity)
    {
        entity.IsDeleted = true;
        
        await UpdateAsync(context, entity);
    }

    public IQueryable<T> GetAll(IContext context)
    {
        var dbContext = (DbContext) context;
        return dbContext.Set<T>().AsQueryable<T>().AsNoTracking();
    }

    public async Task UpdateAsync(IContext context, T entity)
    {
        var dbContext = (DbContext) context;
        entity.UpdatedDateTime = DateTime.UtcNow;
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }
}