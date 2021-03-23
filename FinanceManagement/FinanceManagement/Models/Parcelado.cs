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

        public Periodo Periodo { get; set; }

        public FixoParcelado? FixoParcelados { get; set; }
    }
}