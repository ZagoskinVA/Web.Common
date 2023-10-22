using Microsoft.EntityFrameworkCore;
using WebUtilities.Model;

namespace WebUtilities.Interfaces;

public interface IRepository<T> where T : BaseObject
{
    public Task UpdateAsync(IContext context, T entity);
    public Task DeleteAsync(IContext context, T entity);
    public IQueryable<T> GetAll(IContext context);
    public Task<string> CreateAsync(IContext context, T entity);
}