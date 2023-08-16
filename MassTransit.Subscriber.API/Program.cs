using MassTransit;
using MassTransit.Subscriber.API.Subscribers;
using MessagingEvents.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddMassTransit(c =>
{
    c.AddConsumer<CustomerCreatedSubscriber>();

    c.UsingRabbitMq((context, config) =>
    {
        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapGet("/", () => "Subscriber is working!");

app.Run();