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
    public class ParceladosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParceladosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Parcelados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parcelado>>> GetParcelados()
        {
            return await _context.Parcelados.ToListAsync();
        }

        // GET: api/Parcelados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parcelado>> GetParcelado(int id)
        {
            var parcelado = await _context.Parcelados.FindAsync(id);

            if (parcelado == null)
            {
                return NotFound();
            }

            return parcelado;
        }

        // PUT: api/Parcelados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParcelado(int id, Parcelado parcelado)
        {
            if (id != parcelado.Id)
            {
                return BadRequest();
            }

            _context.Entry(parcelado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParceladoExists(id))
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

        // POST: api/Parcelados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parcelado>> PostParcelado(Parcelado parcelado)
        {
            _context.Parcelados.Add(parcelado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParcelado", new { id = parcelado.Id }, parcelado);
        }

        // DELETE: api/Parcelados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParcelado(int id)
        {
            var parcelado = await _context.Parcelados.FindAsync(id);
            if (parcelado == null)
            {
                return NotFound();
            }

            _context.Parcelados.Remove(parcelado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParceladoExists(int id)
        {
            return _context.Parcelados.Any(e => e.Id == id);
        }
    }
}
