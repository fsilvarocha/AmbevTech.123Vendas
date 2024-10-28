using System.ComponentModel.DataAnnotations;

namespace AmbevTech._123Vendas.Domain.Entidades;

public class Venda
{
    public Venda()
    {
        Itens = new();
    }

    [Key]
    public int NumeroVenda { get; set; }
    public DateTime DataVenda { get; set; } = DateTime.Now;
    public string Cliente { get; set; }
    public decimal ValorTotal { get; set; }
    public string Filial { get; set; }
    public List<ItemVenda> Itens { get; set; }
    public bool Cancelado { get; set; } = false;
}
