using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using WebUtilities.Interfaces;
using WebUtilities.Model;

namespace WebUtilities.EventBus.Abstract;

public class ProducerBase<T>: IRabbitMqProducer<T> where T: IEvent, new()
{
    private string ExchangeName { get; }
    private string RoutingKeyName { get; }
    private string QueueName { get; }

    private readonly ConnectionFactory _connectionFactory;
    private readonly ILogger _logger;

    public ProducerBase(ConnectionFactory connectionFactory, ILogger<ProducerBase<T>> logger)
    {
        var @event = new T();
        ExchangeName = @event.ExchangeName;
        RoutingKeyName = @event.RoutingKeyName;
        QueueName = @event.QueueName;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    public void Publish<TU>(TU entity)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            Declare(channel);
            var message = new EventMessage()
            {
                Type = typeof(TU).Name,
                Entity = JsonConvert.SerializeObject(entity)
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var properties = channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 1; // Doesn't persist to disk
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKeyName, body: body,
                basicProperties: properties);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    private void Declare(IModel channel)
    {
        channel.ExchangeDeclare(
            exchange: ExchangeName, 
            type: "direct", 
            durable: true, 
            autoDelete: false);
                
        channel.QueueDeclare(
            queue: QueueName, 
            durable: true,
            exclusive: false, 
            autoDelete: false);
                
        channel.QueueBind(
            queue: QueueName, 
            exchange: ExchangeName, 
            routingKey: RoutingKeyName);
    }

}