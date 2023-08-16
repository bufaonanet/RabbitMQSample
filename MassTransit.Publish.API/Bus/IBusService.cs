namespace MassTransit.Publish.API.Bus;

public interface IBusService
{
    Task Publish<T>(T message);
}