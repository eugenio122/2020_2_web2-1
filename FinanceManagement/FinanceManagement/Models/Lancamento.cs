using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManagement.Models
{
    public class Lancamento
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Descricao { get; set; }

        public double Valor { get; set; }

        public DateTime Data { get; set; }

        public bool DespesaReceita { get; set; }

        public Usuario Usuario { get; set; }

        public Categoria Categoria { get; set; }

        public FixoParcelado FixoParcelado { get; set; }

        public Conta Conta { get; set; }
    }
}