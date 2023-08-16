using MassTransit;
using MassTransit.Publish.API.Bus;
using MassTransit.Publish.API.Models;
using MessagingEvents.Shared.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBusService, MassTransitBusService>();
builder.Services.AddMassTransit(c =>
{
    c.UsingRabbitMq((context, config) =>
    {
        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/customers", (
    [FromServices] IBusService busService,
    [FromBody] CustomerInputModel model) =>
{
    var @event = new CustomerCreated
    (
        model.Id,
        model.FullName,
        model.Email,
        model.PhoneNumber,
        model.BirthDate
    );

    busService.Publish(@event);

    return Results.NoContent();
});


app.Run();