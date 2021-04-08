using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManagement.Models
{
    public class Parcelado
    {
        [Key]
        public int Id { get; set; }

        public int Quantidade { get; set; }

        public int PeriodoId { get; set; }
        public Periodo Periodo { get; set; }

        public ICollection<Lancamento>? Lancamentos { get; set; }
    }
}