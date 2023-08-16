using MessagingEvents.Shared.Models;
using MessagingEvents.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using RabbitMQClient.Publish.API.Bus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBusService, RabbitMqClientService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/customers", (
    [FromServices] IBusService busService, 
    [FromBody] CustomerInputModel model) =>
{
    const string ROUTING_KEY = "customer-created";

    var @event = new CustomerCreated(model.Id, model.FullName, model.Email, model.PhoneNumber, model.BirthDate);

    busService.Publish(ROUTING_KEY, @event);

    return Results.NoContent();
});

app.Run();