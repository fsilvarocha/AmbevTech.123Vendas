using AmbevTech._123Vendas.Domain.Entidades;

namespace AmbevTech._123Vendas.Domain.Interfaces;

public interface IVendaRepository : IDisposable
{
    Task AddAsync(Venda venda);
    Task UpdateAsync(Venda venda);
    Task DeleteAsync(int idVenda);
    Task<Venda> GetByIdAsync(int id);
    IQueryable<Venda> GetAll();
}
