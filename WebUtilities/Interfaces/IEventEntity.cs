namespace WebUtilities.Interfaces;

public interface IEventEntity
{
    string ExchangeName { get; set; }
    string QueueName { get; set; }
    string RoutingKey { get; set; }
}