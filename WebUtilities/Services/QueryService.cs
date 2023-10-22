using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebUtilities.Interfaces;
using WebUtilities.Model;

namespace WebUtilities.Services;

public class QueryService<T>: IQueryService<T> where T: BaseObject
{
    private readonly ILogger _logger;
    private readonly IRepository<T> _repository;

    public QueryService(ILogger<QueryService<T>> logger, IRepository<T> repository)
    {
        _logger = logger;
        _repository = repository;
    }


    public virtual IQueryable<T> GetAll(IDataContext context)
    {
        _logger.LogInformation($"Get entities: {typeof(T).Name}");
        return _repository.GetAll(context.Context);
    }
}