using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Models
{
    public class CategoriaLancamento
    {
        [Key]
        public int Id { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public int LancamentoId { get; set; }
        public Lancamento Lancamento { get; set; }
    }
}
