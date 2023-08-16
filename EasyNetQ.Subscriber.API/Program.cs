using EasyNetQ;
using EasyNetQ.Subscriber.API.Subscribers;
using MessagingEvents.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<INotificationService, NotificationService>();

var bus = RabbitHutch.CreateBus("host=localhost");
builder.Services.AddSingleton(bus);

builder.Services.AddHostedService<CustomerCreatedSubscriber>();

var app = builder.Build();

app.MapGet("/", () => "Subscriber is working!");

app.Run();