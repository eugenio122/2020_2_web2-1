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
    public class FixoParceladosController : Controller
    {
        private readonly FinanceManagementContext _context;

        public FixoParceladosController(FinanceManagementContext context)
        {
            _context = context;
        }

        // GET: FixoParcelados
        public async Task<IActionResult> Index()
        {
            var financeManagementContext = _context.FixoParcelados.Include(f => f.Fixo).Include(f => f.Parcelado);
            return View(await financeManagementContext.ToListAsync());
        }

        // GET: FixoParcelados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixoParcelado = await _context.FixoParcelados
                .Include(f => f.Fixo)
                .Include(f => f.Parcelado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixoParcelado == null)
            {
                return NotFound();
            }

            return View(fixoParcelado);
        }

        // GET: FixoParcelados/Create
        public IActionResult Create()
        {
            ViewData["FixoId"] = new SelectList(_context.Fixos, "Id", "Id");
            ViewData["ParceladoId"] = new SelectList(_context.Parcelados, "Id", "Id");
            return View();
        }

        // POST: FixoParcelados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FixoId,ParceladoId")] FixoParcelado fixoParcelado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixoParcelado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FixoId"] = new SelectList(_context.Fixos, "Id", "Id", fixoParcelado.FixoId);
            ViewData["ParceladoId"] = new SelectList(_context.Parcelados, "Id", "Id", fixoParcelado.ParceladoId);
            return View(fixoParcelado);
        }

        // GET: FixoParcelados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixoParcelado = await _context.FixoParcelados.FindAsync(id);
            if (fixoParcelado == null)
            {
                return NotFound();
            }
            ViewData["FixoId"] = new SelectList(_context.Fixos, "Id", "Id", fixoParcelado.FixoId);
            ViewData["ParceladoId"] = new SelectList(_context.Parcelados, "Id", "Id", fixoParcelado.ParceladoId);
            return View(fixoParcelado);
        }

        // POST: FixoParcelados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FixoId,ParceladoId")] FixoParcelado fixoParcelado)
        {
            if (id != fixoParcelado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fixoParcelado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixoParceladoExists(fixoParcelado.Id))
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
            ViewData["FixoId"] = new SelectList(_context.Fixos, "Id", "Id", fixoParcelado.FixoId);
            ViewData["ParceladoId"] = new SelectList(_context.Parcelados, "Id", "Id", fixoParcelado.ParceladoId);
            return View(fixoParcelado);
        }

        // GET: FixoParcelados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixoParcelado = await _context.FixoParcelados
                .Include(f => f.Fixo)
                .Include(f => f.Parcelado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixoParcelado == null)
            {
                return NotFound();
            }

            return View(fixoParcelado);
        }

        // POST: FixoParcelados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fixoParcelado = await _context.FixoParcelados.FindAsync(id);
            _context.FixoParcelados.Remove(fixoParcelado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FixoParceladoExists(int id)
        {
            return _context.FixoParcelados.Any(e => e.Id == id);
        }
    }
}
