using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Models
{
    public class Conta
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string DescConta { get; set; }

        public double Saldo { get; set; }

        public Banco Banco { get; set; }

        public TipoConta TipoConta { get; set; }

        public ApplicationUser Usuario { get; set; }

        public ICollection<ContaLancamento> ContaLancamentos { get; set; }
    }
}
