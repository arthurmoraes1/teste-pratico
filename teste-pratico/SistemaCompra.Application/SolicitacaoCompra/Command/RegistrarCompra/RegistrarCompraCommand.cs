using MediatR;
using System;
using System.Collections.Generic;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommand : IRequest<bool>
    {
        public string UsuarioSolicitante { get; set; }
        public string NomeFornecedor { get; set; }
        public int CondicaoPagamento { get; set; }
        public IList<RegistrarCompraItemCommand> Itens { get; set; }
    }

    public class RegistrarCompraItemCommand
    {
        public Guid ProductId { get; set; }
        public int Qtde { get; set; }
    }
}
