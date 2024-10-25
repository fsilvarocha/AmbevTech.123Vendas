using AmbevTech._123Vendas.Domain.Entidades;
using AmbevTech._123Vendas.Domain.Events.Base;

namespace AmbevTech._123Vendas.Domain.Events;

public class CompraCriada : DomainEvent
{
    public Venda Venda { get; private set; }
    public CompraCriada(Venda venda) => Venda = venda;
}
