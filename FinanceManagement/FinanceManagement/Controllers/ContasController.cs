using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceManagement.Data;
using FinanceManagement.Models;
using System.Security.Claims;

namespace FinanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContasController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUsuarioLogado()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/Contas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conta>>> GetContas()
        {
            var userId = this.GetUsuarioLogado();
            
            return await _context.Contas.Where(x => x.Usuario.Id == userId).ToListAsync();
        }

        // GET: api/Contas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Conta>> GetConta(int id)
        {
            var userId = this.GetUsuarioLogado();
            var conta = await _context.Contas.Where(x => x.Usuario.Id == userId && x.Id == id).FirstOrDefaultAsync();

            if (conta == null)
            {
                return NotFound();
            }

            return conta;
        }

        // PUT: api/Contas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConta(int id, [FromBody] Conta conta)
        {
            if (id != conta.Id)
            {
                return BadRequest();
            }

            _context.Entry(conta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
            /*this.deletar(id);

            var userId = this.GetUsuarioLogado();
            var listLancamento = await _context.Lancamentos.Where(x => x.Usuario.Id == userId).ToListAsync();


            this.salvar(conta);

            return NoContent();*/
        }

        // POST: api/Contas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Conta>> PostConta(Conta conta)
        {
            var userId = this.GetUsuarioLogado();
            var usuario = await _context.Usuarios.FindAsync(userId);
            conta.Usuario = usuario;
            
            _context.Contas.Add(conta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConta", new { id = conta.Id }, conta);
        }

        // DELETE: api/Contas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConta(int id)
        {
            var conta = await _context.Contas.FindAsync(id);
            if (conta == null)
            {
                return NotFound();
            }

            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void salvar(Conta conta)
        {
            var userId = this.GetUsuarioLogado();
            var usuario =  _context.Usuarios.Find(userId);
            conta.Usuario = usuario;

            _context.Contas.Add(conta);
            _context.SaveChanges();
        }

        private void deletar(int id)
        {
            Conta conta = _context.Contas.Find(id);

            _context.Contas.Remove(conta);
            _context.SaveChanges();
        }

        private bool ContaExists(int id)
        {
            return _context.Contas.Any(e => e.Id == id);
        }
    }
}
