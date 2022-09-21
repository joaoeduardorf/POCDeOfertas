namespace Precos.Admin.API.Models
{
    public class Preco
    {
        public int PrecoId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataEHora { get; set; } = DateTime.Now;
        
    }
}
