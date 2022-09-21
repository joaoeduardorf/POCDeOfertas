namespace Descontos.Admin.API.Models
{
    public class Desconto
    {
        public int DescontoId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Percentual { get; set; }
        public TipoDePagamento TipoDePagamento { get; set; }
        public DateTime DataEHora { get; set; } = DateTime.UtcNow;
    }
}
