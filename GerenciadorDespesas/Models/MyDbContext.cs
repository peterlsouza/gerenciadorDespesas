using GerenciadorDespesas.Mapeamento;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDespesas.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<Meses> Meses { get; set; }
        public DbSet<Salarios> Salarios { get; set; }
        public DbSet<Despesas> Despesas { get; set; }
        public DbSet<TipoDespesas> TipoDespesas { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext>options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TipoDespesasMap());
            modelBuilder.ApplyConfiguration(new DespesasMap());
            modelBuilder.ApplyConfiguration(new SalariosMap());
            modelBuilder.ApplyConfiguration(new MesesMap());
        }
    }
}
