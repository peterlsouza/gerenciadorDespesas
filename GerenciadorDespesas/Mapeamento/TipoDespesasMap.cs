using GerenciadorDespesas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorDespesas.Mapeamento
{
    public class TipoDespesasMap : IEntityTypeConfiguration<TipoDespesas>
    {
        public void Configure(EntityTypeBuilder<TipoDespesas> builder)
        {
            builder.HasKey(td => td.TipoDespesaId);
            builder.Property(td => td.Nome).HasMaxLength(50).IsRequired();

            builder.HasMany(td => td.Despesas).WithOne(td => td.TipoDespesas).HasForeignKey(td => td.TipoDespesaId);

            builder.ToTable("TipoDespesas");
        }
    }
}
