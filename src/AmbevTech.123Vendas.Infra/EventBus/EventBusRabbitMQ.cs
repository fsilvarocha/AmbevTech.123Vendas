using AmbevTech._123Vendas.Application.Interfaces;
using AmbevTech._123Vendas.Domain.Events.Base;
using Tingle.EventBus;

namespace AmbevTech._123Vendas.Infra.EventBus;

public class EventBusRabbitMQ : IEventBus
{
    private readonly IEventPublisher _eventPublisher;

    public EventBusRabbitMQ(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task PublishAsync(DomainEvent @event)
    {
        var eventContext = _eventPublisher.CreateEventContext(@event);
        await _eventPublisher.PublishAsync(eventContext);
    }
}
