using AmbevTech._123Vendas.API.Controllers;
using AmbevTech._123Vendas.Application.Interfaces;
using AmbevTech._123Vendas.Domain.Entidades;
using AmbevTech._123Vendas.Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AmbevTech._123Vendas.SandBox.Tests.Controller;

public class VendasControllerTests
{
    private readonly Mock<IVendaService> _mockVendaService;
    private readonly VendasController _controller;

    public VendasControllerTests(Mock<IVendaService> mockVendaService, VendasController controller)
    {
        _mockVendaService = mockVendaService;
        _controller = controller;
    }


    [Fact]
    public async Task CreateVenda_ReturnaCreatedAtActionResult_QuandoVendaCriada()
    {
        // Arrange
        Venda venda = new();
        Venda vendaCriada = new();
        _mockVendaService.Setup(service => service.CreateVendaAsync(venda)).ReturnsAsync(vendaCriada);

        // Act
        var result = await _controller.CreateVenda(venda);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Venda>(actionResult.Value);
        Assert.Equal(vendaCriada.NumeroVenda, returnValue.NumeroVenda);
    }

    [Fact]
    public async Task CreateVenda_ReturnaBadRequest_QuandoVendaExistente()
    {
        // Arrange
        Venda venda = new() { NumeroVenda = 1 };
        var errorMessage = $"Venda {venda.NumeroVenda} já existente!";
        _mockVendaService.Setup(service => service.CreateVendaAsync(venda)).ThrowsAsync(new BusinessException(errorMessage));

        // Act
        var result = await _controller.CreateVenda(venda);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(errorMessage, actionResult.Value);
    }

    [Fact]
    public async Task UpdateVenda_NumeroVendaInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        var mockService = new Mock<IVendaService>();
        var controller = new VendasController(mockService.Object);
        var venda = new Venda { NumeroVenda = 0 };

        // Act
        var result = await controller.UpdateVenda(1, venda);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Número da venda não informado!", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateVenda_ValidVenda_DeveRetornarOk()
    {
        // Arrangevar
        var venda = new Venda { NumeroVenda = 1 };
        _mockVendaService.Setup(service => service.UpdateVendaAsync(It.IsAny<Venda>())).Returns(Task.FromResult(venda));
        var controller = new VendasController(_mockVendaService.Object);


        // Act
        var result = await controller.UpdateVenda(1, venda);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateVenda_BusinessException_DeveRetornarBadRequest()
    {
        // Arrange
        var venda = new Venda { NumeroVenda = 1 };
        _mockVendaService.Setup(service => service.UpdateVendaAsync(It.IsAny<Venda>())).ThrowsAsync(new BusinessException($"Venda {venda.NumeroVenda} não existe!"));
        var controller = new VendasController(_mockVendaService.Object);

        // Act
        var result = await controller.UpdateVenda(1, venda);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal($"Venda {venda.NumeroVenda} não existe!", badRequestResult.Value);
    }

    [Fact]
    public async Task CancelarVenda_DeveRetornarOk_QuandoSucesso()
    {
        // Arrange
        _mockVendaService.Setup(service => service.CancelVendaAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
        var controller = new VendasController(_mockVendaService.Object);

        // Act
        var result = await controller.CancelVenda(1);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CancelarVenda_DeveRetornarBadRequest_QuandoBusinessException()
    {
        // Arrange
        var venda = new Venda { NumeroVenda = 1 };
        _mockVendaService.Setup(service => service.CancelVendaAsync(It.IsAny<int>())).ThrowsAsync(new BusinessException($"Venda {venda.NumeroVenda} não existe!"));
        var controller = new VendasController(_mockVendaService.Object);

        // Act
        var result = await controller.CancelVenda(1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal($"Venda {venda.NumeroVenda} não existe!", badRequestResult.Value);
    }

    [Fact]
    public async Task CancelItem_DeveRetornarOk_QuandoCancelamentoForBemSucedido()
    {
        // Arrange
        int id = 1;
        int itemId = 1;
        _mockVendaService.Setup(service => service.CancelItemAsync(id, itemId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CancelItem(id, itemId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CancelItem_DeveRetornarBadRequest_QuandoBusinessExceptionForLancada()
    {
        // Arrange
        int id = 1;
        int itemId = 1;
        string mensagemErro = $"Item {itemId} não existe na venda {id}";
        _mockVendaService.Setup(service => service.CancelItemAsync(id, itemId)).ThrowsAsync(new BusinessException(mensagemErro));

        // Act
        var result = await _controller.CancelItem(id, itemId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(mensagemErro, badRequestResult.Value);
    }
}
