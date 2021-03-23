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
    public class FixosController : Controller
    {
        private readonly FinanceManagementContext _context;

        public FixosController(FinanceManagementContext context)
        {
            _context = context;
        }

        // GET: Fixos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fixos.ToListAsync());
        }

        // GET: Fixos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixo = await _context.Fixos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixo == null)
            {
                return NotFound();
            }

            return View(fixo);
        }

        // GET: Fixos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fixos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao")] Fixo fixo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fixo);
        }

        // GET: Fixos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixo = await _context.Fixos.FindAsync(id);
            if (fixo == null)
            {
                return NotFound();
            }
            return View(fixo);
        }

        // POST: Fixos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao")] Fixo fixo)
        {
            if (id != fixo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fixo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixoExists(fixo.Id))
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
            return View(fixo);
        }

        // GET: Fixos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixo = await _context.Fixos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixo == null)
            {
                return NotFound();
            }

            return View(fixo);
        }

        // POST: Fixos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fixo = await _context.Fixos.FindAsync(id);
            _context.Fixos.Remove(fixo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FixoExists(int id)
        {
            return _context.Fixos.Any(e => e.Id == id);
        }
    }
}
