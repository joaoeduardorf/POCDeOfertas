using CalculoDeOfertas.Consumer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Domain
{
    public class CalcularOfertas
    {
        private readonly Preco _preco;
        private readonly List<Desconto> _descontos;
        private readonly List<Oferta> _ofertas;

        public CalcularOfertas(Preco preco, List<Desconto> descontos)
        {
            this._preco = preco;
            this._descontos = descontos;
            _ofertas = new List<Oferta>();
        }

        public List<Oferta> ObterOfertas()
        {
            Oferta ofertaBase = new Oferta();
            if (!_descontos.Any(a => a.TipoDePagamento == 0))
            {
                Oferta oferta = new Oferta();
                oferta = CalcularOfertaBase();
                _ofertas.Add(oferta);
            }

            foreach (var desconto in _descontos)
            {
                Oferta oferta = new Oferta();
                oferta = CalcularOferta(desconto);
                _ofertas.Add(oferta);
            }

            return _ofertas;

        }

        public Oferta CalcularOferta(Desconto desconto)
        {
            Oferta oferta = new Oferta();
            oferta.ProdutoId = _preco.ProdutoId;
            oferta.ValorPreco = _preco.Valor;
            oferta.PercentualDesconto = desconto.Percentual;
            oferta.TipoDePagamento = desconto.TipoDePagamento;
            oferta.ValorOferta = _preco.Valor * Math.Abs((desconto.Percentual / 100) - 1);

            return oferta;
        }

        public Oferta CalcularOfertaBase()
        {
            Oferta oferta = new Oferta();
            oferta.ProdutoId = _preco.ProdutoId;
            oferta.ValorPreco = _preco.Valor;
            oferta.PercentualDesconto = 0;
            oferta.TipoDePagamento = TipoDePagamento.NENHUMA;
            oferta.ValorOferta = _preco.Valor;

            return oferta;
        }
    }
}
