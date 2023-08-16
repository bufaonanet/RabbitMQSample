using EasyNetQ;
using EasyNetQ.Publish.API.Bus;
using EasyNetQ.Publish.API.Models;
using MessagingEvents.Shared.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var bus = RabbitHutch.CreateBus("host=localhost");
builder.Services.AddSingleton<IBusService, EasyNetQService>(sp => new EasyNetQService(bus));

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