using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceManagement.Data;
using FinanceManagement.Models.ViewModels;
using System.Security.Claims;

namespace FinanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentoFixoViewModelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LancamentoFixoViewModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUsuarioLogado()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/LancamentoFixoViewModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LancamentoFixoViewModel>>> GetLancamentoFixoViewModel()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = this.GetUsuarioLogado();

                List<LancamentoFixoViewModel> lancamentoViewModel = new List<LancamentoFixoViewModel>();
                var lista = (from lanc in _context.Lancamentos
                             join user in _context.Usuarios on lanc.Usuario.Id equals user.Id
                             join catl in _context.CategoriaLancamentos on lanc.Id equals catl.LancamentoId
                             join cate in _context.Categorias on catl.CategoriaId equals cate.Id
                             join conl in _context.ContaLancamentos on lanc.Id equals conl.LancamentoId
                             join cont in _context.Contas on conl.ContaId equals cont.Id
                             join fixo in _context.Fixos on lanc.FixoId equals fixo.Id

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
                                 cont.DescConta,
                                 fixo.DescFixo
                             }
                            ).Distinct().OrderBy(x => x.Data).ToList();

                foreach (var item in lista)
                {
                    LancamentoFixoViewModel lvm = new LancamentoFixoViewModel();
                    lvm.Id = item.Id;
                    lvm.Descricao = item.Descricao;
                    lvm.Valor = item.Valor;
                    lvm.Data = item.Data;
                    lvm.DespesaReceita = item.DespesaReceita;
                    lvm.TipoLancamento = item.TipoLancamento;
                    lvm.Categoria = item.DescCategoria;
                    lvm.Conta = item.DescConta;
                    lvm.Fixo = item.DescFixo;
                    lancamentoViewModel.Add(lvm);
                }

                return lancamentoViewModel;
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/LancamentoFixoViewModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LancamentoFixoViewModel>> GetLancamentoFixoViewModel(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = this.GetUsuarioLogado();

                var lista = (from lanc in _context.Lancamentos
                             join user in _context.Usuarios on lanc.Usuario.Id equals user.Id
                             join catl in _context.CategoriaLancamentos on lanc.Id equals catl.LancamentoId
                             join cate in _context.Categorias on catl.CategoriaId equals cate.Id
                             join conl in _context.ContaLancamentos on lanc.Id equals conl.LancamentoId
                             join cont in _context.Contas on conl.ContaId equals cont.Id
                             join fixo in _context.Fixos on lanc.FixoId equals fixo.Id

                             where (user.Id == userId) && (lanc.Id == id)

                             select new
                             {
                                 lanc.Id,
                                 lanc.Descricao,
                                 lanc.Valor,
                                 lanc.Data,
                                 lanc.DespesaReceita,
                                 lanc.TipoLancamento,
                                 cate.DescCategoria,
                                 cont.DescConta,
                                 fixo.DescFixo
                             }
                            ).Distinct().OrderBy(x => x.Data).ToList();

                LancamentoFixoViewModel lvm = new LancamentoFixoViewModel();
                foreach (var item in lista)
                {
                    lvm.Id = item.Id;
                    lvm.Descricao = item.Descricao;
                    lvm.Valor = item.Valor;
                    lvm.Data = item.Data;
                    lvm.DespesaReceita = item.DespesaReceita;
                    lvm.TipoLancamento = item.TipoLancamento;
                    lvm.Categoria = item.DescCategoria;
                    lvm.Conta = item.DescConta;
                    lvm.Fixo = item.DescFixo;
                }

                return lvm;
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: api/LancamentoFixoViewModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutLancamentoFixoViewModel(int id, LancamentoFixoViewModel lancamentoFixoViewModel)
        {
            if (id != lancamentoFixoViewModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(lancamentoFixoViewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LancamentoFixoViewModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/LancamentoFixoViewModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       /* [HttpPost]
        public async Task<ActionResult<LancamentoFixoViewModel>> PostLancamentoFixoViewModel(LancamentoFixoViewModel lancamentoFixoViewModel)
        {
            _context.LancamentoFixoViewModel.Add(lancamentoFixoViewModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLancamentoFixoViewModel", new { id = lancamentoFixoViewModel.Id }, lancamentoFixoViewModel);
        }*/

        // DELETE: api/LancamentoFixoViewModel/5
       /* [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLancamentoFixoViewModel(int id)
        {
            var lancamentoFixoViewModel = await _context.LancamentoFixoViewModel.FindAsync(id);
            if (lancamentoFixoViewModel == null)
            {
                return NotFound();
            }

            _context.LancamentoFixoViewModel.Remove(lancamentoFixoViewModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LancamentoFixoViewModelExists(int id)
        {
            return _context.LancamentoFixoViewModel.Any(e => e.Id == id);
        }*/
    }
}
