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

        [DataType(DataType.Currency)]
        public double Valor { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        public bool DespesaReceita { get; set; }

        public ApplicationUser Usuario { get; set; }

        public Categoria Categoria { get; set; }

        public FixoParcelado FixoParcelado { get; set; }

        public Conta Conta { get; set; }
    }
}