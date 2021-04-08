using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceManagement.Data;
using FinanceManagement.Models;

namespace FinanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaLancamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriaLancamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CategoriaLancamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaLancamento>>> GetCategoriaLancamentos()
        {
            return await _context.CategoriaLancamentos.ToListAsync();
        }

        // GET: api/CategoriaLancamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaLancamento>> GetCategoriaLancamento(int id)
        {
            var categoriaLancamento = await _context.CategoriaLancamentos.FindAsync(id);

            if (categoriaLancamento == null)
            {
                return NotFound();
            }

            return categoriaLancamento;
        }

        // PUT: api/CategoriaLancamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoriaLancamento(int id, CategoriaLancamento categoriaLancamento)
        {
            if (id != categoriaLancamento.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoriaLancamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaLancamentoExists(id))
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

        // POST: api/CategoriaLancamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoriaLancamento>> PostCategoriaLancamento(CategoriaLancamento categoriaLancamento)
        {
            _context.CategoriaLancamentos.Add(categoriaLancamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoriaLancamento", new { id = categoriaLancamento.Id }, categoriaLancamento);
        }

        // DELETE: api/CategoriaLancamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaLancamento(int id)
        {
            var categoriaLancamento = await _context.CategoriaLancamentos.FindAsync(id);
            if (categoriaLancamento == null)
            {
                return NotFound();
            }

            _context.CategoriaLancamentos.Remove(categoriaLancamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaLancamentoExists(int id)
        {
            return _context.CategoriaLancamentos.Any(e => e.Id == id);
        }
    }
}
