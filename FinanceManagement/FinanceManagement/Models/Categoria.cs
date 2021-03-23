using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Descricao { get; set; }

        public ICollection<Lancamento> Lancamentos { get; set; }
    }
}
