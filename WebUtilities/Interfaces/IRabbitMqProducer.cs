namespace WebUtilities.Interfaces;

public interface IRabbitMqProducer<T> where T: IEvent
{
    public void Publish<U>(U @event);
}