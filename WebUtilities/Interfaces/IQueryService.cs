using WebUtilities.Model;

namespace WebUtilities.Interfaces;

public interface IQueryService<out T> where T: BaseObject
{
    IQueryable<T> GetAll(IDataContext context);
}