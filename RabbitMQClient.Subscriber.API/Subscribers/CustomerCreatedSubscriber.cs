using System.Text;
using System.Text.Json;
using MessagingEvents.Shared.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQClient.Subscriber.API.Subscribers;

public class CustomerCreatedSubscriber : BackgroundService
{
    private readonly IModel _channel;

    const string EXCHANGE = "curso-rabbitmq";
    const string CUSTOMER_CREATED_QUEUE = "customer";

    public CustomerCreatedSubscriber()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection("RabbitMQClient.Subscriber.API");

        _channel = connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);

            var @event = JsonSerializer.Deserialize<CustomerCreated>(contentString);

            Console.WriteLine($"Message received: {contentString}");
            Console.WriteLine($"CustomerCreated received: {@event}");

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(CUSTOMER_CREATED_QUEUE, false, consumer);

        return Task.CompletedTask;
    }
}