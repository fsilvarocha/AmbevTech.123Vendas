using AmbevTech._123Vendas.Domain.Entidades;

namespace AmbevTech._123Vendas.Application.Interfaces;

public interface IVendaService
{
    Task<Venda> CreateVendaAsync(Venda venda);
    Task<Venda> UpdateVendaAsync(Venda venda);
    Task CancelVendaAsync(int numeroVenda);
    Task CancelItemAsync(int numeroVenda, int itemId);
    IQueryable<Venda> GetVendaListAsync();
    Task<Venda> GetVendaByIdAsync(int id);
}
