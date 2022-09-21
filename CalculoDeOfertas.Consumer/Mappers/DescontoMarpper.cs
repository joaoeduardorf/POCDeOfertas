using CalculoDeOfertas.Consumer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Mappers
{
    public static class DescontoMarpper
    {
        public static List<Desconto> Mapper(this List<DescontoAPI> descontosAPI)
        {
            List<Desconto> list = new List<Desconto>();
            foreach (var descontoAPI in descontosAPI)
            {
                list.Add(descontoAPI.Mapper());
            }

            return list;
        }
        public static Desconto Mapper(this DescontoAPI descontoAPI)
        {
            return new Desconto
            {
                ProdutoId = descontoAPI.produtoId,
                DescontoId = descontoAPI.descontoId,
                Percentual = descontoAPI.percentual,
                TipoDePagamento = descontoAPI.tipoDePagamento
            };
        }

        public static List<DescontoAPI> Mapper(this List<Desconto> descontos)
        {
            List<DescontoAPI> list = new List<DescontoAPI>();
            foreach (var desconto in descontos)
            {
                list.Add(desconto.Mapper());
            }

            return list;
        }
        public static DescontoAPI Mapper(this Desconto desconto)
        {
            return new DescontoAPI
            {
                produtoId = desconto.ProdutoId,
                descontoId = desconto.DescontoId,
                percentual = desconto.Percentual,
                tipoDePagamento = desconto.TipoDePagamento
            };
        }
    }
}
