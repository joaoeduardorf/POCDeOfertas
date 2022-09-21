using MongoDB.Bson;

namespace Ofertas.API.Models
{
    public class Oferta
    {
        public ObjectId _id { get; set; }
        public int OfertaId { get; set; }
        public int ProdutoId { get; set; }
        public decimal ValorPreco { get; set; }
        public decimal PercentualDesconto { get; set; }
        public TipoDePagamento TipoDePagamento { get; set; }
        public decimal ValorOferta { get; set; }
        public DateTime DataEHora { get; set; }
    }
}
