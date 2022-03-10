using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra : Entity
    {
        public UsuarioSolicitante UsuarioSolicitante { get; private set; }
        public NomeFornecedor NomeFornecedor { get; private set; }
        public IList<Item> Itens { get; private set; }
        public DateTime Data { get; private set; }
        public Money TotalGeral { get; private set; }
        public Situacao Situacao { get; private set; }
        public CondicaoPagamento CondicaoPagamento { get; private set; }

        private SolicitacaoCompra() { }

        public SolicitacaoCompra(string usuarioSolicitante, string nomeFornecedor, int condicaoPagamento)
        {
            Id = Guid.NewGuid();
            UsuarioSolicitante = new UsuarioSolicitante(usuarioSolicitante);
            NomeFornecedor = new NomeFornecedor(nomeFornecedor);
            Data = DateTime.Now;
            Situacao = Situacao.Solicitado;
            TotalGeral = new Money();
            CondicaoPagamento = new CondicaoPagamento(condicaoPagamento);
        }

        public void AdicionarItem(Produto produto, int qtde)
        {
            Itens.Add(new Item(produto, qtde));
        }

        public void RegistrarCompra(IEnumerable<Item> itens)
        {
            if (itens != null)
            {
                foreach (var item in itens)
                {
                    if (item.Produto.Situacao != ProdutoAggregate.Situacao.Ativo) 
                        throw new BusinessRuleException("Produto deve estar ativo!");
                    if (item.Qtde <= 0) 
                        throw new BusinessRuleException("A quantidade deve ser maior que zero!");

                    Itens.Add(item);
                    TotalGeral = TotalGeral.Add(item.Subtotal);
                }
            }

            if (TotalGeral.Value > 50000m && CondicaoPagamento.Valor != 30) 
                throw new BusinessRuleException("A condição de pagamento deve ser 30 dias!");

            if (Itens.Count() == 0) 
                throw new BusinessRuleException("A quantidade de itens deve ser maior que 0");

            AddEvent(new CompraRegistradaEvent(Id, Itens, TotalGeral.Value));
        }
    }
}
