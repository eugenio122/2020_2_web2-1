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
    public class TipoContasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TipoContasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TipoContas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoConta>>> GetTipoContas()
        {
            return await _context.TipoContas.ToListAsync();
        }

        // GET: api/TipoContas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoConta>> GetTipoConta(int id)
        {
            var tipoConta = await _context.TipoContas.FindAsync(id);

            if (tipoConta == null)
            {
                return NotFound();
            }

            return tipoConta;
        }

        // PUT: api/TipoContas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoConta(int id, TipoConta tipoConta)
        {
            if (id != tipoConta.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoConta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoContaExists(id))
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

        // POST: api/TipoContas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoConta>> PostTipoConta(TipoConta tipoConta)
        {
            _context.TipoContas.Add(tipoConta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoConta", new { id = tipoConta.Id }, tipoConta);
        }

        // DELETE: api/TipoContas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoConta(int id)
        {
            var tipoConta = await _context.TipoContas.FindAsync(id);
            if (tipoConta == null)
            {
                return NotFound();
            }

            _context.TipoContas.Remove(tipoConta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoContaExists(int id)
        {
            return _context.TipoContas.Any(e => e.Id == id);
        }
    }
}
