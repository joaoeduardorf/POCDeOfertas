using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Models
{
    public class DescontoAPI
    {
        public int descontoId { get; set; }
        public int produtoId { get; set; }
        public decimal percentual { get; set; }
        public TipoDePagamento tipoDePagamento { get; set; }
    }
}
