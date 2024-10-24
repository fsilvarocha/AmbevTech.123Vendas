namespace AmbevTech._123Vendas.Domain.Entidades;

public class Venda : BaseEntities
{
    public DateTime DataVenda { get; set; } = new DateTime();
    public decimal Valor { get; set; }
    public decimal ValorDesconto { get; set; } = 0;
    public bool ItemCancelado { get; set; } = false;
}
