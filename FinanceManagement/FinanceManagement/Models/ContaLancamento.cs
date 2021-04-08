using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Models
{
    public class ContaLancamento
    {
        [Key]
        public int Id { get; set; }

        public int ContaId { get; set; }
        public Conta Conta { get; set; }

        public int LancamentoId { get; set; }
        public Lancamento Lancamento { get; set; }
    }
}
