using AmbevTech._123Vendas.Domain.Entidades;
using AmbevTech._123Vendas.Domain.Interfaces;
using AmbevTech._123Vendas.Infra.DataContext;
using Microsoft.EntityFrameworkCore;

namespace AmbevTech._123Vendas.Infra.Repositories;

public class VendaRepository : IVendaRepository
{
    private readonly Context _context;

    public VendaRepository(Context context)
    {
        _context = context;
    }

    public async Task AddAsync(Venda venda)
    {
        _context.Vendas.Add(venda);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int idVenda)
    {
        Venda venda = await _context.Vendas.Where(v => v.NumeroVenda == idVenda).FirstOrDefaultAsync();
        if (venda != null)
        {
            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();
        }
    }

    public void Dispose() => _context.Dispose();

    public IQueryable<Venda> GetAll() =>
        _context.Vendas.Include(v => v.Itens);


    public async Task<Venda> GetByIdAsync(int id) =>
        await _context.Vendas.Include(v => v.Itens).FirstOrDefaultAsync(v => v.NumeroVenda == id);


    public async Task UpdateAsync(Venda venda)
    {
        _context.Vendas.Update(venda);
        await _context.SaveChangesAsync();
    }
}
