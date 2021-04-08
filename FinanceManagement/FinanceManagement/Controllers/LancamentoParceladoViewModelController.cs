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
    public class LancamentoParceladoViewModelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LancamentoParceladoViewModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUsuarioLogado()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/LancamentoParceladoViewModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LancamentoParceladoViewModel>>> GetLancamentoParceladoViewModel()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = this.GetUsuarioLogado();

                List<LancamentoParceladoViewModel> lancamentoViewModel = new List<LancamentoParceladoViewModel>();
                var lista = (from lanc in _context.Lancamentos
                             join user in _context.Usuarios on lanc.Usuario.Id equals user.Id
                             join catl in _context.CategoriaLancamentos on lanc.Id equals catl.LancamentoId
                             join cate in _context.Categorias on catl.CategoriaId equals cate.Id
                             join conl in _context.ContaLancamentos on lanc.Id equals conl.LancamentoId
                             join cont in _context.Contas on conl.ContaId equals cont.Id
                             join parc in _context.Parcelados on lanc.ParceladoId equals parc.Id
                             join peri in _context.Periodos on parc.PeriodoId equals peri.Id

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
                                 parc.Quantidade,
                                 peri.DescPeriodo
                             }
                            ).Distinct().OrderBy(x => x.Data).ToList();

                foreach (var item in lista)
                {
                    LancamentoParceladoViewModel lvm = new LancamentoParceladoViewModel();
                    lvm.Id = item.Id;
                    lvm.Descricao = item.Descricao;
                    lvm.Valor = item.Valor;
                    lvm.Data = item.Data;
                    lvm.DespesaReceita = item.DespesaReceita;
                    lvm.TipoLancamento = item.TipoLancamento;
                    lvm.Categoria = item.DescCategoria;
                    lvm.Conta = item.DescConta;
                    lvm.Quantidade = item.Quantidade;
                    lvm.Parcelado = item.DescPeriodo;
                    lancamentoViewModel.Add(lvm);
                }

                return lancamentoViewModel;
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/LancamentoParceladoViewModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LancamentoParceladoViewModel>> GetLancamentoParceladoViewModel(int id)
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
                             join parc in _context.Parcelados on lanc.ParceladoId equals parc.Id
                             join peri in _context.Periodos on parc.PeriodoId equals peri.Id

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
                                 parc.Quantidade,
                                 peri.DescPeriodo
                             }
                            ).Distinct().OrderBy(x => x.Data).ToList();

                LancamentoParceladoViewModel lvm = new LancamentoParceladoViewModel();
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
                    lvm.Quantidade = item.Quantidade;
                    lvm.Parcelado = item.DescPeriodo;
                }

                return lvm;
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: api/LancamentoParceladoViewModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
 /*       [HttpPut("{id}")]
        public async Task<IActionResult> PutLancamentoParceladoViewModel(int id, LancamentoParceladoViewModel lancamentoParceladoViewModel)
        {
            if (id != lancamentoParceladoViewModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(lancamentoParceladoViewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LancamentoParceladoViewModelExists(id))
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

        // POST: api/LancamentoParceladoViewModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
 /*       [HttpPost]
        public async Task<ActionResult<LancamentoParceladoViewModel>> PostLancamentoParceladoViewModel(LancamentoParceladoViewModel lancamentoParceladoViewModel)
        {
            _context.LancamentoParceladoViewModel.Add(lancamentoParceladoViewModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLancamentoParceladoViewModel", new { id = lancamentoParceladoViewModel.Id }, lancamentoParceladoViewModel);
        }*/

        // DELETE: api/LancamentoParceladoViewModel/5
  /*      [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLancamentoParceladoViewModel(int id)
        {
            var lancamentoParceladoViewModel = await _context.LancamentoParceladoViewModel.FindAsync(id);
            if (lancamentoParceladoViewModel == null)
            {
                return NotFound();
            }

            _context.LancamentoParceladoViewModel.Remove(lancamentoParceladoViewModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LancamentoParceladoViewModelExists(int id)
        {
            return _context.LancamentoParceladoViewModel.Any(e => e.Id == id);
        }*/
    }
}
