using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Models
{
    public class Banco
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Nome { get; set; }

        public ICollection<Conta> Contas { get; set; }
    }
}
