using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceManagement.Data;
using FinanceManagement.Models;
using FinanceManagement.Models.ViewModels;

namespace FinanceManagement.Controllers
{
    public class ContasController : Controller
    {
        private readonly FinanceManagementContext _context;

        public ContasController(FinanceManagementContext context)
        {
            _context = context;
        }

        // GET: Contas
        public IActionResult Index()
        {
            return View(this.IndexContaViewModel());
        }

        // GET: Contas/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conta = this.IndexContaViewModel().FirstOrDefault(x => x.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            return View(conta);
        }

        // GET: Contas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Saldo")] Conta conta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conta);
        }

        // GET: Contas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conta = await _context.Contas.FindAsync(id);
            if (conta == null)
            {
                return NotFound();
            }
            return View(conta);
        }

        // POST: Contas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Saldo")] Conta conta)
        {
            if (id != conta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContaExists(conta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(conta);
        }

        // GET: Contas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conta = await _context.Contas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            return View(conta);
        }

        // POST: Contas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conta = await _context.Contas.FindAsync(id);
            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContaExists(int id)
        {
            return _context.Contas.Any(e => e.Id == id);
        }

        private List<ContaViewModel> IndexContaViewModel()
        {
            List<ContaViewModel> lancamentoViewModel = new List<ContaViewModel>();

            var lista = (from con in _context.Contas
                         join ban in _context.Bancos on con.Banco.Id equals ban.Id
                         join tpc in _context.TipoContas on con.TipoConta.Id equals tpc.Id
                         select new
                         {
                             con.Id,
                             con.Descricao,
                             con.Saldo,
                             ban.Nome,
                             tpc.Tipo
                         }
                        ).ToList();


            foreach (var item in lista)
            {
                ContaViewModel cvm = new ContaViewModel();
                cvm.Id = item.Id;
                cvm.DescConta = item.Descricao;
                cvm.Saldo = item.Saldo;
                cvm.DescBanco = item.Nome;
                cvm.TipoConta = item.Tipo;
                lancamentoViewModel.Add(cvm);
            }

            return lancamentoViewModel;
        }
    }
}
