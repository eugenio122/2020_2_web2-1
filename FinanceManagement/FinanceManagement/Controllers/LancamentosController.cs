using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceManagement.Data;
using FinanceManagement.Models;

namespace FinanceManagement.Controllers
{
    public class LancamentosController : Controller
    {
        private readonly FinanceManagementContext _context;

        public LancamentosController(FinanceManagementContext context)
        {
            _context = context;
        }

        // GET: Lancamentoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lancamentos.ToListAsync());
        }

        // GET: Lancamentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamento = await _context.Lancamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lancamento == null)
            {
                return NotFound();
            }

            return View(lancamento);
        }

        private int categoriaId = 0;
        // GET: Lancamentoes/Create
        public IActionResult Create()
        {
            //ViewBag.Categorias = _context.Categorias.Select(c => new SelectListItem() { Text = c.Descricao, Value = c.Id.ToString() }).ToList();

           var categoriasList = _context.Categorias.Select(c => new SelectListItem() { Text = c.Descricao, Value = c.Id.ToString(), Selected = true }).ToList();

            categoriasList.Insert(0, new SelectListItem()
            {
                Text = "Selecione",
                Value = string.Empty
            });

            ViewBag.Categorias = categoriasList;
            
            return View();
        }

        // POST: Lancamentoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Valor,Data,DespesaReceita,Categoria")] Lancamento lancamento)
        {
            

            if (ModelState.IsValid)
            {
                

                _context.Add(lancamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lancamento);
        }

        // GET: Lancamentoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamento = await _context.Lancamentos.FindAsync(id);
            if (lancamento == null)
            {
                return NotFound();
            }
            return View(lancamento);
        }

        // POST: Lancamentoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Valor,Data,DespesaReceita")] Lancamento lancamento)
        {
            if (id != lancamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lancamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LancamentoExists(lancamento.Id))
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
            return View(lancamento);
        }

        // GET: Lancamentoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamento = await _context.Lancamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lancamento == null)
            {
                return NotFound();
            }

            return View(lancamento);
        }

        // POST: Lancamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lancamento = await _context.Lancamentos.FindAsync(id);
            _context.Lancamentos.Remove(lancamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LancamentoExists(int id)
        {
            return _context.Lancamentos.Any(e => e.Id == id);
        }
    }
}
