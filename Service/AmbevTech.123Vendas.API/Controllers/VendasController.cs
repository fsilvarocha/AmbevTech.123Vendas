using AmbevTech._123Vendas.Application.Interfaces;
using AmbevTech._123Vendas.Domain.Entidades;
using AmbevTech._123Vendas.Domain.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmbevTech._123Vendas.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class VendasController : ControllerBase
{
    private readonly IVendaService _vendaService;

    public VendasController(IVendaService vendaService)
    {
        _vendaService = vendaService;
    }

    /// <summary>
    /// Criar venda
    /// </summary>
    /// <param name="venda"></param>
    /// <returns></returns>
    [HttpPost("criar/")]
    public async Task<ActionResult<Venda>> CreateVenda(Venda venda)
    {
        try
        {
            var createdVenda = await _vendaService.CreateVendaAsync(venda);
            return CreatedAtAction(nameof(CreateVenda), new { id = createdVenda.NumeroVenda }, createdVenda);
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Atualizar venda
    /// </summary>
    /// <param name="id"></param>
    /// <param name="venda"></param>
    /// <returns></returns>
    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> UpdateVenda(int id, Venda venda)
    {
        if (venda.NumeroVenda <= 0)
        {
            return BadRequest("Número da venda não informado!");
        }

        try
        {
            await _vendaService.UpdateVendaAsync(venda);
            return Ok();
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletar a venda
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("deletar/{id}")]
    public async Task<IActionResult> CancelVenda(int id)
    {
        try
        {
            await _vendaService.CancelVendaAsync(id);
            return Ok();
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Cancelar o Item de uma venda
    /// </summary>
    /// <param name="id"></param>
    /// <param name="itemId"></param>
    /// <returns></returns>
    [HttpPut("cancelar/{id}/item/{itemId}")]
    public async Task<IActionResult> CancelItem(int id, int itemId)
    {
        try
        {
            await _vendaService.CancelItemAsync(id, itemId);
            return Ok();
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retornar venda pelo Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("consultar/{id}")]
    public async Task<IActionResult> GetVendaById(int id)
    {
        try
        {
            await _vendaService.GetVendaByIdAsync(id);
            return Ok();
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
