using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Models
{
    public class FixoParcelado
    {
        [Key]
        public int Id { get; set; }

        public int? FixoId { get; set; }
        public Fixo Fixo { get; set; }

        public int? ParceladoId { get; set; }
        public Parcelado Parcelado { get; set; }

        public ICollection<Lancamento> Lancamentos { get; set; }
    }
}