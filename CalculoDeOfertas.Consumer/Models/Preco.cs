using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Models
{
    public class Preco
    {
        public int PrecoId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }

    }
}
