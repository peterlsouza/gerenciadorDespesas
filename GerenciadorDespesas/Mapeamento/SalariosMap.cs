using GerenciadorDespesas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorDespesas.Mapeamento
{
    public class SalariosMap : IEntityTypeConfiguration<Salarios>
    {
        public void Configure(EntityTypeBuilder<Salarios> builder)
        {
            builder.HasKey(s => s.SalarioId);
            builder.Property(s => s.Valor).IsRequired();

            builder.HasOne(s => s.Meses).WithOne(s => s.Salarios).HasForeignKey<Salarios>(s => s.MesId);

            builder.ToTable("Salarios");
        }
    }
}
