using Descontos.Admin.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descontos.Consumer.Models
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
