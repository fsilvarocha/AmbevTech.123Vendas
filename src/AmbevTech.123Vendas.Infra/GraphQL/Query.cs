using AmbevTech._123Vendas.Application.Interfaces;
using AmbevTech._123Vendas.Domain.Entidades;

namespace AmbevTech._123Vendas.Infra.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]

    public IQueryable<Venda> vendas([Service] IVendaService vendaService) =>
        vendaService.GetVendaListAsync();
}
