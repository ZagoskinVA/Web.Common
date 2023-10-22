using Microsoft.Extensions.Logging;
using Web.Common.Entity.Entity;
using WebUtilities.EventBus.Events;
using WebUtilities.Interfaces;
using WebUtilities.Services;

namespace Web.Common.Data.Services.CrudServices;

public class DemoObjectCrudService: CrudService<DemoObject>
{
    private readonly IUserProvider _userProvider;
    public DemoObjectCrudService(IRepository<DemoObject> repository, ILogger<CrudService<DemoObject>> logger, IRabbitMqProducer<CreatedEvent> createdEventProducer, IUserProvider userProvider) : base(repository, logger, createdEventProducer)
    {
        _userProvider = userProvider;
    }

    public override Task<string> CreateAsync(ITransactionContext context, DemoObject entity)
    {
        entity.UserId = _userProvider.GetUserId();
        return base.CreateAsync(context, entity);
    }
}