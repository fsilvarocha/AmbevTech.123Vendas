namespace AmbevTech._123Vendas.Domain.Entidades;

public class ItemVenda
{
    public int Id { get; set; }
    public string Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal Desconto { get; set; }
    public decimal ValorTotal => Quantidade * ValorUnitario - Desconto;
    public bool Cancelado { get; set; }

}