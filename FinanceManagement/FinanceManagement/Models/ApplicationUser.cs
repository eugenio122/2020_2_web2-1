using FinanceManagement.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        [MaxLength(250)]
        public string Email { get; set; }

        [MaxLength(250)]
        public string Nome { get; set; }

        [MaxLength(16)]
        public string Senha { get; set; }

        public ApplicationUser? UsuarioDependente { get; set; }

        public ICollection<ApplicationUser> Usuarios { get; set; }

        public ICollection<Lancamento> Lancamentos { get; set; }

        public ICollection<Categoria> Categorias { get; set; }

        public ICollection<Conta> Contas { get; set; }
    }
}
