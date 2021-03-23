using FinanceManagement.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FinanceManagement.Data;

namespace FinanceManagement.Models
{
    public class Usuario : IdentityUser
    {
        [Key]
        [MaxLength(250)]
        public string Email { get; set; }

        [MaxLength(250)]
        public string Nome { get; set; }

        [MaxLength(16)]
        public string Senha { get; set; }

        public Usuario? UsuarioDependente { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }

        public ICollection<Lancamento> Lancamentos { get; set; }
    }
}
