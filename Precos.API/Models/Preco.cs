using MongoDB.Bson;

namespace Precos.API.Models
{
    public class Preco
    {
        public ObjectId _id { get; set; }
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataEHora { get; set; }
    }
}
