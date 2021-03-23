using FinanceManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Data
{
    public class FinanceManagementContext : IdentityDbContext<Usuario>
    {
        public FinanceManagementContext(DbContextOptions<FinanceManagementContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Banco> Bancos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Conta> Contas { get; set; }

        public DbSet<Fixo> Fixos { get; set; }

        public DbSet<FixoParcelado> FixoParcelados { get; set; }

        public DbSet<Lancamento> Lancamentos { get; set; }

        public DbSet<Parcelado> Parcelados { get; set; }

        public DbSet<Periodo> Periodos { get; set; }

        public DbSet<TipoConta> TipoContas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
