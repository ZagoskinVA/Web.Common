using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebUtilities.EventBus.Events;
using WebUtilities.Interfaces;
using WebUtilities.Model;

namespace WebUtilities.Services;

public class CrudService<T> : ICrudService<T> where T : BaseObject
{
    private readonly IRepository<T> _repository;
    private readonly ILogger _logger;
    private readonly IRabbitMqProducer<CreatedEvent> _createdEventProducer;

    public CrudService(IRepository<T> repository, ILogger<CrudService<T>> logger, 
        IRabbitMqProducer<CreatedEvent> createdEventProducer)
    {
        _repository = repository;
        _logger = logger;
        _createdEventProducer = createdEventProducer;
    }

    public virtual async Task<string> CreateAsync(ITransactionContext context, T entity)
    {
        _logger.LogInformation($"Start creating entity: {typeof(T).Name} with ID {entity.Id}");
        context.OnCommittedAsync(() =>
        {
            _createdEventProducer.Publish(entity);
            return Task.CompletedTask;
        });
        var id = await _repository.CreateAsync(context.Context, entity);
        _logger.LogInformation($"Successfully created entity: {typeof(T).Name} with ID {id}");
        return id;
    }

    public virtual async Task UpdateAsync(ITransactionContext context, T entity)
    {
        _logger.LogInformation($"Start updating entity: {typeof(T).Name} with ID {entity.Id}");
        await _repository.UpdateAsync(context.Context, entity);
        context.OnCommittedAsync(() =>
        {
            _createdEventProducer.Publish(entity);
            return Task.CompletedTask;
        });
        _logger.LogInformation($"Successfully updated entity: {typeof(T).Name} with ID {entity.Id}");
    }

    public virtual async Task DeleteAsync(ITransactionContext context, T entity)
    {
        _logger.LogInformation($"Start deleting entity: {typeof(T).Name} with ID {entity.Id}");
        await _repository.DeleteAsync(context.Context, entity);
        _logger.LogInformation($"Start deleted entity: {typeof(T).Name} with ID {entity.Id}");
    }
}