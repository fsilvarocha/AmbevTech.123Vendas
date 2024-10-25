using AmbevTech._123Vendas.Domain.Events.Base;

namespace AmbevTech._123Vendas.Domain.Events;

public class CompraCancelada : DomainEvent
{
    public int NumeroVenda { get; private set; }
    public CompraCancelada(int numeroVenda) => NumeroVenda = numeroVenda;
}
