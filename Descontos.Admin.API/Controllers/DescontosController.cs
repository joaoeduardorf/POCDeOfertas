using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Descontos.Admin.API.Data;
using Descontos.Admin.API.Models;

namespace Descontos.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DescontosController : ControllerBase
    {
        private readonly DescontosAdminAPIContext _context;

        public DescontosController(DescontosAdminAPIContext context)
        {
            _context = context;
        }

        // GET: api/Descontos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Desconto>>> GetDesconto()
        {
            if (_context.Desconto == null)
            {
                return NotFound();
            }
            return await _context.Desconto.ToListAsync();
        }

        // GET: api/Descontos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Desconto>> GetDesconto(int id)
        {
            if (_context.Desconto == null)
            {
                return NotFound();
            }
            var desconto = await _context.Desconto.FindAsync(id);

            if (desconto == null)
            {
                return NotFound();
            }

            return desconto;
        }

        // PUT: api/Descontos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesconto(int id, Desconto desconto)
        {
            if (id != desconto.DescontoId)
            {
                return BadRequest();
            }

            _context.Entry(desconto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                new DescontosAdminAPIKafka().Produzir("Desconto.Atualizar", desconto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DescontoExists(id))
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

        // POST: api/Descontos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Desconto>> PostDesconto(Desconto desconto)
        {
            if (_context.Desconto == null)
            {
                return Problem("Entity set 'DescontosAdminAPIContext.Desconto'  is null.");
            }

            var existeDescontoParaProdutoETipoDePagameto = _context.Desconto.Any(w => w.ProdutoId == desconto.ProdutoId && w.TipoDePagamento == desconto.TipoDePagamento);
            if (!existeDescontoParaProdutoETipoDePagameto)
            {
                _context.Desconto.Add(desconto);
                await _context.SaveChangesAsync();
                new DescontosAdminAPIKafka().Produzir("Desconto.Criar", desconto);
                return CreatedAtAction("GetDesconto", new { id = desconto.DescontoId }, desconto);
            }

            return BadRequest();



        }

        // DELETE: api/Descontos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesconto(int id)
        {
            if (_context.Desconto == null)
            {
                return NotFound();
            }
            var desconto = await _context.Desconto.FindAsync(id);
            if (desconto == null)
            {
                return NotFound();
            }

            _context.Desconto.Remove(desconto);
            await _context.SaveChangesAsync();

            new DescontosAdminAPIKafka().Produzir("Desconto.Apagar", desconto);

            return NoContent();
        }

        private bool DescontoExists(int id)
        {
            return (_context.Desconto?.Any(e => e.DescontoId == id)).GetValueOrDefault();
        }
    }
}
