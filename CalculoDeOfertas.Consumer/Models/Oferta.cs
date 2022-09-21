using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Models
{
    public class Oferta
    {
        public int OfertaId { get; set; }
        public int ProdutoId { get; set; }
        public decimal ValorPreco { get; set; }
        public decimal PercentualDesconto { get; set; }
        public TipoDePagamento TipoDePagamento { get; set; }
        public decimal ValorOferta { get; set; }
        public DateTime DataEHora { get; set; }
    }
}
