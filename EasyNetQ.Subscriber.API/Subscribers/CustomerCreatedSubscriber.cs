using MessagingEvents.Shared.Services;
using Newtonsoft.Json;

namespace EasyNetQ.Subscriber.API.Subscribers;

public class CustomerCreatedSubscriber : BackgroundService
{
    private const string CUSTOMER_CREATED_QUEUE = "customer";

    private readonly IServiceProvider _serviceProvider;
    private readonly IAdvancedBus _bus;

    public CustomerCreatedSubscriber(IServiceProvider serviceProvider, IBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus.Advanced;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queue = _bus.QueueDeclare(CUSTOMER_CREATED_QUEUE, stoppingToken);

        _bus.Consume<CustomerCreated>(queue, async (msg, info) =>
        {
            var json = JsonConvert.SerializeObject(msg.Body);

            await SendEmail(msg.Body);
            Console.WriteLine($"Message Received: {json}");
        });

        return Task.CompletedTask;
    }

    private async Task SendEmail(CustomerCreated @event)
    {
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<INotificationService>();

        await service.SendEmail(
            @event.Email,
            CUSTOMER_CREATED_QUEUE,
            new Dictionary<string, string> { { "name", @event.FullName } }
        );
    }
}