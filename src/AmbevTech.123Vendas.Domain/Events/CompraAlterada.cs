using AmbevTech._123Vendas.Domain.Entidades;
using AmbevTech._123Vendas.Domain.Events.Base;

namespace AmbevTech._123Vendas.Domain.Events;

public class CompraAlterada : DomainEvent
{
    public Venda Venda { get; private set; }
    public CompraAlterada(Venda venda) => Venda = venda;
}
