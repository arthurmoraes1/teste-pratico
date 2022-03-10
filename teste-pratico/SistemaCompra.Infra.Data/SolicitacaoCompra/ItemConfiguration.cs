using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolicitacaoCompraAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Infra.Data.SolicitacaoCompra
{
    public class ItemConfiguration : IEntityTypeConfiguration<SolicitacaoCompraAgg.Item>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCompraAgg.Item> builder)
        {
            builder.ToTable("Item");
            builder.HasOne(c => c.Produto).WithMany(x => x.Itens).HasForeignKey("ProdutoId");
            builder.HasOne(c => c.SolicitacaoCompra).WithMany(c => c.Itens).HasForeignKey("SolicitacaoCompraId");
        }
    }
}
