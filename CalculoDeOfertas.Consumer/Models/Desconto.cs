using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Models
{
    public class Desconto
    {
        public ObjectId _id { get; set; }
        public int DescontoId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Percentual { get; set; }
        public TipoDePagamento TipoDePagamento { get; set; }
    }
}
