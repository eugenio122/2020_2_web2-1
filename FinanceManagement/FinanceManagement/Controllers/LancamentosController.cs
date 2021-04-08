using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceManagement.Data;
using FinanceManagement.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using FinanceManagement.Models.ViewModels;

namespace FinanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LancamentosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        // GET: api/Lancamentos
        [HttpGet]
        public ActionResult<IEnumerable<LancamentoViewModel>> GetLancamentos()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = this.GetUsuarioLogado();

                List<LancamentoViewModel> lancamentoViewModel = new List<LancamentoViewModel>();
                var lista = (from lanc in _context.Lancamentos
                             join user in _context.Usuarios on lanc.Usuario.Id equals user.Id
                             join catl in _context.CategoriaLancamentos on lanc.Id equals catl.LancamentoId
                             join cate in _context.Categorias on catl.CategoriaId equals cate.Id
                             join conl in _context.ContaLancamentos on lanc.Id equals conl.LancamentoId
                             join cont in _context.Contas on conl.ContaId equals cont.Id

                             where (user.Id == userId)

                             select new
                             {
                                 lanc.Id,
                                 lanc.Descricao,
                                 lanc.Valor,
                                 lanc.Data,
                                 lanc.DespesaReceita,
                                 lanc.TipoLancamento,
                                 cate.DescCategoria,
                                 cont.DescConta
                             }
                            ).Distinct().OrderBy(x => x.Data).ToList();

                foreach (var item in lista)
                {
                    LancamentoViewModel lvm = new LancamentoViewModel();
                    lvm.Id = item.Id;
                    lvm.Descricao = item.Descricao;
                    lvm.Valor = item.Valor;
                    lvm.Data = item.Data;
                    lvm.DespesaReceita = item.DespesaReceita;
                    lvm.TipoLancamento = item.TipoLancamento;
                    lvm.Categoria = item.DescCategoria;
                    lvm.Conta = item.DescConta;

                    lancamentoViewModel.Add(lvm);
                }

                return lancamentoViewModel;
            }
            else
            {
                return NotFound();
            }
        }

        private string GetUsuarioLogado()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/Lancamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lancamento>> GetLancamento(int id)
        {
            var lancamento = await _context.Lancamentos.FindAsync(id);

            if (lancamento == null)
            {
                return NotFound();
            }

            return lancamento;
        }

        // PUT: api/Lancamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLancamento(int id, Lancamento lancamento)
        {
            if (id != lancamento.Id)
            {
                return BadRequest();
            }

            _context.Entry(lancamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LancamentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Lancamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lancamento>> PostLancamento(Lancamento lancamento)
        {
            var userId = this.GetUsuarioLogado();
            var usuario = await _context.Usuarios.FindAsync(userId);

            lancamento.Usuario = usuario;

            _context.Lancamentos.Add(lancamento);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetLancamento", new { id = lancamento.Id }, lancamento);
        }

        // DELETE: api/Lancamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLancamento(int id)
        {
            var lancamento = await _context.Lancamentos.FindAsync(id);
            if (lancamento == null)
            {
                return NotFound();
            }

            _context.Lancamentos.Remove(lancamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LancamentoExists(int id)
        {
            return _context.Lancamentos.Any(e => e.Id == id);
        }

        [HttpGet("{Email}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetLancamentosByUser(string email)
        {
            var lancamento = await _context.Lancamentos.Where(x => x.Usuario.Email == email).ToListAsync();

            if (lancamento == null)
            {
                return NotFound();
            }

            return lancamento;
        }
    }
}
