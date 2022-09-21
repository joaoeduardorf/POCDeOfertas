using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Precos.Consumer.Models
{
    public class Preco
    {
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataEHora { get; set; }
    }
}
