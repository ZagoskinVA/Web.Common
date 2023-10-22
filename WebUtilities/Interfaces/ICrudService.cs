using Microsoft.EntityFrameworkCore;
using WebUtilities.Model;

namespace WebUtilities.Interfaces;

public interface ICrudService<T> where T : BaseObject
{
    Task<string> CreateAsync(ITransactionContext context, T entity);

    Task UpdateAsync(ITransactionContext context, T entity);

    Task DeleteAsync(ITransactionContext context, T entity);
}