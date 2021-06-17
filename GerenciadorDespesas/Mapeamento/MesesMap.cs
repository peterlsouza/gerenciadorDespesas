using GerenciadorDespesas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorDespesas.Mapeamento
{
    public class MesesMap : IEntityTypeConfiguration<Meses>
    {
        public void Configure(EntityTypeBuilder<Meses> builder)
        {
            builder.HasKey(m => m.MesId);
            builder.Property(m => m.MesId).ValueGeneratedNever(); //bd nao vai gerar valor para este atributo
            builder.Property(m => m.Nome).HasMaxLength(50).IsRequired();

            builder.HasMany(m => m.Despesas).WithOne(m => m.Meses).HasForeignKey(m => m.MesId).OnDelete(DeleteBehavior.Cascade);//se excluir um mes deletara todas despesas tbm...
            builder.HasOne(m => m.Salarios).WithOne(m => m.Meses).HasForeignKey<Salarios>(m => m.MesId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Meses");
        }
    }
}
