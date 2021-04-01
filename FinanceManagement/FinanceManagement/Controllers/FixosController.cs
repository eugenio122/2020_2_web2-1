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
    public class FixosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FixosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Fixos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fixo>>> GetFixos()
        {
            return await _context.Fixos.ToListAsync();
        }

        // GET: api/Fixos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fixo>> GetFixo(int id)
        {
            var fixo = await _context.Fixos.FindAsync(id);

            if (fixo == null)
            {
                return NotFound();
            }

            return fixo;
        }

        // PUT: api/Fixos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFixo(int id, Fixo fixo)
        {
            if (id != fixo.Id)
            {
                return BadRequest();
            }

            _context.Entry(fixo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FixoExists(id))
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

        // POST: api/Fixos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Fixo>> PostFixo(Fixo fixo)
        {
            _context.Fixos.Add(fixo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFixo", new { id = fixo.Id }, fixo);
        }

        // DELETE: api/Fixos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFixo(int id)
        {
            var fixo = await _context.Fixos.FindAsync(id);
            if (fixo == null)
            {
                return NotFound();
            }

            _context.Fixos.Remove(fixo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FixoExists(int id)
        {
            return _context.Fixos.Any(e => e.Id == id);
        }
    }
}
