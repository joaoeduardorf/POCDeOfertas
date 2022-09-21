using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Models
{
    public class PrecoAPI
    {
        public int precoId { get; set; }
        public int produtoId { get; set; }
        public decimal valor { get; set; }
    }
}
