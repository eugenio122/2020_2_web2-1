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
    public class ParceladosController : Controller
    {
        private readonly FinanceManagementContext _context;

        public ParceladosController(FinanceManagementContext context)
        {
            _context = context;
        }

        // GET: Parcelados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parcelados.ToListAsync());
        }

        // GET: Parcelados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelado = await _context.Parcelados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcelado == null)
            {
                return NotFound();
            }

            return View(parcelado);
        }

        // GET: Parcelados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parcelados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantidade")] Parcelado parcelado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parcelado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parcelado);
        }

        // GET: Parcelados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelado = await _context.Parcelados.FindAsync(id);
            if (parcelado == null)
            {
                return NotFound();
            }
            return View(parcelado);
        }

        // POST: Parcelados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantidade")] Parcelado parcelado)
        {
            if (id != parcelado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcelado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParceladoExists(parcelado.Id))
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
            return View(parcelado);
        }

        // GET: Parcelados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcelado = await _context.Parcelados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcelado == null)
            {
                return NotFound();
            }

            return View(parcelado);
        }

        // POST: Parcelados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parcelado = await _context.Parcelados.FindAsync(id);
            _context.Parcelados.Remove(parcelado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParceladoExists(int id)
        {
            return _context.Parcelados.Any(e => e.Id == id);
        }
    }
}
