using System.Reflection;
using WebUtilities.Interfaces;
using WebUtilities.Model;

namespace WebUtilities.EventBus.Events;

public record CreatedEvent: IEvent
{
    public string ExchangeName =>  $"CreatedCrudExchange";

    public string RoutingKeyName => $"CreatedCrudCommand";

    public string QueueName => $"CreatedCrudQueue";
}