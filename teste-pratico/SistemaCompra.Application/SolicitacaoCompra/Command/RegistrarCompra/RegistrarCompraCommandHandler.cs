using MediatR;
using SistemaCompra.Infra.Data.UoW;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolicitacaoCompraAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommandHandler : CommandHandler, IRequestHandler<RegistrarCompraCommand, bool>
    {
        private readonly SolicitacaoCompraAgg.ISolicitacaoCompraRepository _solicitacaoCompraRepository;
        private readonly ProdutoAgg.IProdutoRepository _produtoRepository;

        public RegistrarCompraCommandHandler(SolicitacaoCompraAgg.ISolicitacaoCompraRepository solicitacaoCompraRepository, ProdutoAgg.IProdutoRepository produtoRepository, IUnitOfWork uow, IMediator mediator) : base(uow, mediator)
        {
            this._solicitacaoCompraRepository = solicitacaoCompraRepository;
            this._produtoRepository = produtoRepository;
        }

        public Task<bool> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            var compra = new SolicitacaoCompraAgg.SolicitacaoCompra(request.UsuarioSolicitante, request.NomeFornecedor, request.CondicaoPagamento);

            var itens = new List<SolicitacaoCompraAgg.Item>();
            foreach (var item in request.Itens)
            {
                var produto = _produtoRepository.Obter(item.ProductId);
                itens.Add(new SolicitacaoCompraAgg.Item(produto, item.Qtde));
            }
            compra.RegistrarCompra(itens);
            _solicitacaoCompraRepository.RegistrarCompra(compra);

            Commit();
            PublishEvents(compra.Events);

            return Task.FromResult(true);
        }
    }
}
