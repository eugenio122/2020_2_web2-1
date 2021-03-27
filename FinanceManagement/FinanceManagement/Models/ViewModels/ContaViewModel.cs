using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Models.ViewModels
{
    public class ContaViewModel
    {
        public int Id { get; set; }

        public string DescConta { get; set; }

        public double Saldo { get; set; }

        public string DescBanco { get; set;}

        public string TipoConta { get; set; }
    }
}
