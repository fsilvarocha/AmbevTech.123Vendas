using AmbevTech._123Vendas.Domain.Events.Base;

namespace AmbevTech._123Vendas.Application.Interfaces;

public interface IEventBus
{
    Task PublishAsync(DomainEvent @event);
}
