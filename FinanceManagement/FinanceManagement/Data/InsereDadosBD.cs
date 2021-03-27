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
        private readonly FinanceManagementContext context;

        public InsereDadosBD(FinanceManagementContext context)
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

            this.context.SaveChanges();
        }

        private void AddRoles()
        {
            var roleResponsavel = new IdentityRole { Name = "Responsável", NormalizedName = "RESPONSAVEL" };
            var roleDependente = new IdentityRole { Name = "Dependente", NormalizedName = "DEPENDENTE" };
            this.context.Roles.AddRange(roleResponsavel, roleDependente);
        }

        private void AddUsuario()
        {
            var usuario = new Usuario { Nome = "Admin", Email = "admin@admin.com", Senha = "admin@2021" };
            var dependente = new Usuario { Nome = "Dependente", Email = "dependente@gmail.com", Senha = "admin@2021" };
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
            this.context.Bancos.AddRange(bradesco, itau, banese, caixa, santander);
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
            var diario = new Fixo { Descricao = "Diário" };
            var semanal = new Fixo { Descricao = "Semanal" };
            var quinzenal = new Fixo { Descricao = "Quinzenal" };
            var mensal = new Fixo { Descricao = "Mensal" };
            var bimestral = new Fixo { Descricao = "Bimestral" };
            var trimestral = new Fixo { Descricao = "Trimestral" };
            var semestral = new Fixo { Descricao = "Semestral" };
            var anual = new Fixo { Descricao = "Anual" };
            this.context.Fixos.AddRange(diario, semanal, quinzenal, mensal, bimestral, trimestral, semestral, anual);
        }

        private void AddPeriodo()
        {
            var dias = new Periodo { Descricao = "Dias" };
            var semanas = new Periodo { Descricao = "Semanas" };
            var quinzenas = new Periodo { Descricao = "Quinzenas" };
            var meses = new Periodo { Descricao = "Meses" };
            var bimestres = new Periodo { Descricao = "Bimestres" };
            var trimestres = new Periodo { Descricao = "Trimestres" };
            var semestres = new Periodo { Descricao = "Semestres" };
            var anos = new Periodo { Descricao = "Anos" };
            this.context.Periodos.AddRange(dias, semanas, quinzenas, meses, bimestres, trimestres, semestres, anos);
        }

        
    }
}
