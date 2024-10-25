using AmbevTech._123Vendas.Application.Interfaces;
using AmbevTech._123Vendas.Domain.Entidades;
using AmbevTech._123Vendas.Domain.Events;
using AmbevTech._123Vendas.Domain.Exception;
using AmbevTech._123Vendas.Domain.Interfaces;
using Serilog;

namespace AmbevTech._123Vendas.Application.Services;

public class VendaService : IVendaService
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IEventBus _eventBus;

    public VendaService(IVendaRepository vendaRepository, IEventBus eventBus)
    {
        _vendaRepository = vendaRepository;
        _eventBus = eventBus;
    }

    public async Task<Venda> CreateVendaAsync(Venda venda)
    {
        Log.Warning($"Venda {venda.NumeroVenda} entrou no metodo {nameof(CreateVendaAsync)}");

        Venda vendaDb = await _vendaRepository.GetByIdAsync(venda.NumeroVenda);
        if (vendaDb is not null)
            throw new BusinessException($"Venda {venda.NumeroVenda} já existente!");

        await _vendaRepository.AddAsync(venda);
        await _eventBus.PublishAsync(new CompraCriada(venda));

        Log.Warning($"Venda {venda.NumeroVenda} criada!");
        return venda;
    }

    public async Task<Venda> UpdateVendaAsync(Venda venda)
    {
        Log.Warning($"Venda {venda.NumeroVenda} entrou no metodo {nameof(UpdateVendaAsync)}");

        var vendaDb = await ValidarVendaExistente(venda.NumeroVenda);
        await _vendaRepository.UpdateAsync(venda);
        await _eventBus.PublishAsync(new CompraAlterada(venda));

        Log.Warning($"Venda {venda.NumeroVenda} alterada!");
        return venda;
    }

    public async Task CancelVendaAsync(int numeroVenda)
    {
        Log.Warning($"Venda {numeroVenda} entrou no metodo {nameof(CancelVendaAsync)}");

        var venda = await ValidarVendaExistente(numeroVenda);
        venda.Cancelado = true;
        await _vendaRepository.UpdateAsync(venda);
        await _eventBus.PublishAsync(new CompraCancelada(numeroVenda));

        Log.Warning($"Venda {venda.NumeroVenda} cancelada!");
    }

    public async Task CancelItemAsync(int numeroVenda, int itemId)
    {
        Log.Warning($"Venda {numeroVenda} Pedido {itemId} entrou no metodo {nameof(CancelItemAsync)}!");

        var venda = await ValidarVendaExistente(numeroVenda);
        bool itemExiste = venda.Itens.Any(i => i.Id == itemId);

        if (!itemExiste)
            throw new BusinessException($"Item {itemId} não existe na venda {numeroVenda}");

        venda.Itens.FirstOrDefault(x => x.Id == itemId).Cancelado = true;
        await _vendaRepository.UpdateAsync(venda);
        await _eventBus.PublishAsync(new ItemCancelado(itemId));

        Log.Warning($"Venda {numeroVenda} Pedido {itemId} cancelado!");
    }

    private async Task<Venda> ValidarVendaExistente(int vendaId)
    {
        Venda venda = await _vendaRepository.GetByIdAsync(vendaId);

        if (venda is null)
            throw new BusinessException($"Venda {vendaId} não existe!");

        return venda;
    }

    public IQueryable<Venda> GetVendaListAsync() => _vendaRepository.GetAll();

    public async Task<Venda> GetVendaByIdAsync(int id) => await _vendaRepository.GetByIdAsync(id);

}
