using AmbevTech._123Vendas.Domain.Events.Base;

namespace AmbevTech._123Vendas.Domain.Events;

public class ItemCancelado : DomainEvent
{
    public int ItemId { get; private set; }
    public ItemCancelado(int itemId) => ItemId = itemId;
}
