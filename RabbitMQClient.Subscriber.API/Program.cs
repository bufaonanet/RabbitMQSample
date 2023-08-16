using RabbitMQClient.Subscriber.API.Subscribers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<CustomerCreatedSubscriber>();

var app = builder.Build();

app.MapGet("/", () => "Executing !!!");

app.Run();