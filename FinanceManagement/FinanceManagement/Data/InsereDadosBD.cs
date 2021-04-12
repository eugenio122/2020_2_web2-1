using FinanceManagement.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Data
{
    public class InsereDadosBD
    {
        private readonly ApplicationDbContext context;

        public InsereDadosBD(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Inicializar()
        {
            if (context.Usuarios.Any())
            {
                return;
            }

            this.AddRoles();

            this.AddUsuario();

            this.AddCategorias();

            this.AddBancos();

            this.AddTipoConta();

            this.AddFixo();

            this.AddPeriodo();

            this.AddContas();

            this.context.SaveChanges();
        }

        private TipoConta outros;

        private void AddRoles()
        {
            var roleResponsavel = new IdentityRole { Name = "Responsável", NormalizedName = "RESPONSAVEL" };
            var roleDependente = new IdentityRole { Name = "Dependente", NormalizedName = "DEPENDENTE" };
            this.context.Roles.AddRange(roleResponsavel, roleDependente);
        }

        private void AddUsuario()
        {
            var usuario = new ApplicationUser { Nome = "Admin", Email = "admin@admin.com", Senha = "admin@2021", EmailConfirmed = true };
            var dependente = new ApplicationUser { Nome = "Dependente", Email = "dependente@gmail.com", Senha = "admin@2021", EmailConfirmed = true };
            this.context.Usuarios.AddRange(usuario, dependente);
        }

        private void AddCategorias()
        {
            var lazer = new Categoria { DescCategoria = "Lazer" };
            var compras = new Categoria { DescCategoria = "Compras" };
            var internet = new Categoria { DescCategoria = "Internet" };
            var Assinatura = new Categoria { DescCategoria = "Assinatura" };
            this.context.Categorias.AddRange(lazer, compras, internet, Assinatura);
        }

        private void AddBancos()
        {
            var bradesco = new Banco { Nome = "Bradesco" };
            var itau = new Banco { Nome = "Itaú" };
            var banese = new Banco { Nome = "Banese" };
            var caixa = new Banco { Nome = "Caixa" };
            var santander = new Banco { Nome = "Santander" };
            var nubank = new Banco { Nome = "Nubank" };
            var inter = new Banco { Nome = "Inter" };
            var bb = new Banco { Nome = "Banco do Brasil" };
            this.context.Bancos.AddRange(bradesco, itau, banese, caixa, santander, nubank, inter, bb);
        }

        private void AddTipoConta()
        {
            var corrente = new TipoConta { Tipo = "Conta Corrente" };
            var poupanca = new TipoConta { Tipo = "Conta Poupança" };
            var investimento = new TipoConta { Tipo = "Investimentos" };
            var outros = new TipoConta { Tipo = "Outros" };
            this.context.TipoContas.AddRange(corrente, poupanca, investimento, outros);
        }

        private void AddFixo()
        {
            var diario = new Fixo { DescFixo = "Diário" };
            var semanal = new Fixo { DescFixo = "Semanal" };
            var quinzenal = new Fixo { DescFixo = "Quinzenal" };
            var mensal = new Fixo { DescFixo = "Mensal" };
            var bimestral = new Fixo { DescFixo = "Bimestral" };
            var trimestral = new Fixo { DescFixo = "Trimestral" };
            var semestral = new Fixo { DescFixo = "Semestral" };
            var anual = new Fixo { DescFixo = "Anual" };
            this.context.Fixos.AddRange(diario, semanal, quinzenal, mensal, bimestral, trimestral, semestral, anual);
        }

        private void AddPeriodo()
        {
            var dias = new Periodo { DescPeriodo = "Dias" };
            var semanas = new Periodo { DescPeriodo = "Semanas" };
            var quinzenas = new Periodo { DescPeriodo = "Quinzenas" };
            var meses = new Periodo { DescPeriodo = "Meses" };
            var bimestres = new Periodo { DescPeriodo = "Bimestres" };
            var trimestres = new Periodo { DescPeriodo = "Trimestres" };
            var semestres = new Periodo { DescPeriodo = "Semestres" };
            var anos = new Periodo { DescPeriodo = "Anos" };
            this.context.Periodos.AddRange(dias, semanas, quinzenas, meses, bimestres, trimestres, semestres, anos);
        }

        private void AddContas()
        {
            
        }

    }
}
