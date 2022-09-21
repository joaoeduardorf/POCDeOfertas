using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Descontos.API.Models
{
    [BsonIgnoreExtraElements]
    public class Desconto
    {
        //public ObjectId _id { get; set; }
        public int DescontoId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Percentual { get; set; }
        public TipoDePagamento TipoDePagamento { get; set; }
        public DateTime DataEHora { get; set; } = DateTime.UtcNow;
    }
}
