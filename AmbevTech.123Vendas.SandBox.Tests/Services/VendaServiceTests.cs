using AmbevTech._123Vendas.Application.Interfaces;
using AmbevTech._123Vendas.Application.Services;
using AmbevTech._123Vendas.Domain.Entidades;
using AmbevTech._123Vendas.Domain.Events;
using AmbevTech._123Vendas.Domain.Exception;
using AmbevTech._123Vendas.Domain.Interfaces;
using Moq;

namespace AmbevTech._123Vendas.SandBox.Tests.Services;

public class VendaServiceTests
{
    private readonly Mock<IVendaRepository> _vendaRepositoryMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly VendaService _vendaService;

    public VendaServiceTests()
    {
        _vendaRepositoryMock = new Mock<IVendaRepository>();
        _eventBusMock = new Mock<IEventBus>();
        _vendaService = new VendaService(_vendaRepositoryMock.Object, _eventBusMock.Object);
    }

    [Fact]
    public async Task CreateVendaAsync_RetornaBusinessException_QuandoVendaExistente()
    {
        // Arrange
        var venda = new Venda { NumeroVenda = 1 };
        _vendaRepositoryMock.Setup(repo => repo.GetByIdAsync(venda.NumeroVenda))
                            .ReturnsAsync(venda);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() => _vendaService.CreateVendaAsync(venda));
        Assert.Equal($"Venda {venda.NumeroVenda} já existente!", exception.Message);
    }

    [Fact]
    public async Task CreateVendaAsync_AdicionaVendaEPublicaEvento_QuandoVendaNaoExiste()
    {
        // Arrange
        var venda = new Venda { NumeroVenda = 1 };
        _vendaRepositoryMock.Setup(repo => repo.GetByIdAsync(venda.NumeroVenda)).ReturnsAsync((Venda)null);

        // Act
        var result = await _vendaService.CreateVendaAsync(venda);

        // Assert
        _vendaRepositoryMock.Verify(repo => repo.AddAsync(venda), Times.Once);
        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<CompraCriada>()), Times.Once);
        Assert.Equal(venda, result);
    }

    [Fact]
    public async Task UpdateVendaAsync_AlteraVenda_QuandoEncontrada()
    {
        // Arrange
        var venda = new Venda { NumeroVenda = 123 };
        _vendaRepositoryMock.Setup(repo => repo.GetByIdAsync(venda.NumeroVenda)).ReturnsAsync(venda);
        _vendaRepositoryMock.Setup(repo => repo.UpdateAsync(venda)).Returns(Task.CompletedTask);
        _eventBusMock.Setup(bus => bus.PublishAsync(It.IsAny<CompraAlterada>())).Returns(Task.CompletedTask);

        // Act
        var result = await _vendaService.UpdateVendaAsync(venda);

        // Assert
        _vendaRepositoryMock.Verify(repo => repo.UpdateAsync(venda), Times.Once);
        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<CompraAlterada>()), Times.Once);
        Assert.Equal(venda, result);
    }

    [Fact]
    public async Task UpdateVendaAsync_RetornaException_QuandoVendaNaoEncontrada()
    {
        // Arrange
        var venda = new Venda { NumeroVenda = 123 };
        _vendaRepositoryMock.Setup(repo => repo.GetByIdAsync(venda.NumeroVenda)).ReturnsAsync((Venda)null);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessException>(() => _vendaService.UpdateVendaAsync(venda));
    }

    [Fact]
    public async Task CancelVendaAsync_CancelaVendaEPublicaEvento()
    {
        // Arrange
        int numeroVenda = 123;
        var venda = new Venda { NumeroVenda = numeroVenda };
        _vendaRepositoryMock.Setup(repo => repo.GetByIdAsync(numeroVenda)).ReturnsAsync(venda);
        _vendaRepositoryMock.Setup(repo => repo.UpdateAsync(venda)).Returns(Task.CompletedTask);

        // Act
        await _vendaService.CancelVendaAsync(numeroVenda);

        // Assert
        Assert.True(venda.Cancelado);
    }

    [Fact]
    public async Task CancelItemAsync_ItemExiste_Sucess()
    {
        // Arrange
        int numeroVenda = 1;
        int itemId = 1;
        var venda = new Venda
        {
            NumeroVenda = numeroVenda,
            Itens = new List<ItemVenda>
            {
                new ItemVenda { Id = itemId, Cancelado = false }
            }
        };

        _vendaRepositoryMock.Setup(repo => repo.GetByIdAsync(numeroVenda)).ReturnsAsync(venda);

        // Act
        await _vendaService.CancelItemAsync(numeroVenda, itemId);

        // Assert
        var item = venda.Itens.FirstOrDefault(i => i.Id == itemId);
        Assert.NotNull(item);
        Assert.True(item.Cancelado);
        _vendaRepositoryMock.Verify(repo => repo.UpdateAsync(venda), Times.Once);
        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<ItemCancelado>()), Times.Once);
    }

    [Fact]
    public async Task CancelItemAsync_ItemNaoExiste_RetornaException()
    {
        // Arrange
        int numeroVenda = 1;
        int itemId = 1;
        var venda = new Venda
        {
            NumeroVenda = numeroVenda,
            Itens = new List<ItemVenda>()
        };

        _vendaRepositoryMock.Setup(repo => repo.GetByIdAsync(numeroVenda)).ReturnsAsync(venda);

        // Act && Assert
        await Assert.ThrowsAsync<BusinessException>(() => _vendaService.CancelItemAsync(numeroVenda, itemId));

        _vendaRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Venda>()), Times.Never);
        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<ItemCancelado>()), Times.Never);
    }
}
