using MessagingEvents.Shared.Services;

namespace MassTransit.Subscriber.API.Subscribers;

public class CustomerCreatedSubscriber : IConsumer<CustomerCreated>
{
    private IServiceProvider ServiceProvider { get; }

    public CustomerCreatedSubscriber(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }


    public async  Task Consume(ConsumeContext<CustomerCreated> context)
    {
        var @event = context.Message;

        using var scope = ServiceProvider.CreateScope();

        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        await notificationService.SendEmail
        (
            @event.Email,
            "boas-vindas",
            new Dictionary<string, string> { { "name", @event.FullName } }
        );
    }
}