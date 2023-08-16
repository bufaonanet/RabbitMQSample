namespace MassTransit.Publish.API.Bus;

public class MassTransitBusService : IBusService
{
    private readonly IBus _bus;

    public MassTransitBusService(IBus bus)
    {
        _bus = bus;
    }

    public async Task Publish<T>(T message)
    {
        await _bus.Publish(message);
    }
}