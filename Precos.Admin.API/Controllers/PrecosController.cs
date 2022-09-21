using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Precos.Admin.API.Data;
using Precos.Admin.API.Models;

namespace Precos.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecosController : ControllerBase
    {
        private readonly PrecosAdminAPIContext _context;

        public PrecosController(PrecosAdminAPIContext context)
        {
            _context = context;
        }

        // GET: api/Precos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Preco>>> GetPreco()
        {
            if (_context.Preco == null)
            {
                return NotFound();
            }
            return await _context.Preco.ToListAsync();
        }

        // GET: api/Precos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Preco>> GetPreco(int id)
        {
            if (_context.Preco == null)
            {
                return NotFound();
            }
            var preco = await _context.Preco.FindAsync(id);

            if (preco == null)
            {
                return NotFound();
            }

            return preco;
        }

        [HttpGet("Produto/{id}")]
        public async Task<ActionResult<IEnumerable<Preco>>> GetPrecoProduto(int produtoId)
        {
            if (_context.Preco == null)
            {
                return NotFound();
            }
            return await _context.Preco.Where(w => w.ProdutoId == produtoId).ToListAsync();
        }

        // PUT: api/Precos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPreco(int id, Preco preco)
        //{
        //    if (id != preco.ProdutoId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(preco).State = EntityState.Modified;

        //    var config = new ProducerConfig
        //    {
        //        BootstrapServers = "localhost:9092"
        //    };

        //    using (var producer = new ProducerBuilder<string, string>(config).Build())
        //    {
        //        var result = await producer.ProduceAsync("Preco.Atualizar", new Message<string, string> { Key = JsonSerializer.Serialize(new { preco.ProdutoId }), Value = JsonSerializer.Serialize(preco) });
        //    }

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PrecoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Precos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Preco>> PostPreco(Preco preco)
        {
            if (_context.Preco == null)
            {
                return Problem("Entity set 'PrecosAdminAPIContext.Preco'  is null.");
            }

            if(preco == null)
            {
                return BadRequest();
            }

            var existePrecoParaProduto = _context.Preco.Any(a => a.ProdutoId == preco.ProdutoId);
            _context.Preco.Add(preco);
            await _context.SaveChangesAsync();

            if (!existePrecoParaProduto)
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092"
                };

                using (var producer = new ProducerBuilder<string, string>(config).Build())
                {
                    var result = await producer.ProduceAsync("Preco.Criar", new Message<string, string> { Key = JsonSerializer.Serialize(new { preco.ProdutoId }), Value = JsonSerializer.Serialize(preco) });
                }

                
            }
            else
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092"
                };

                using (var producer = new ProducerBuilder<string, string>(config).Build())
                {
                    var result = await producer.ProduceAsync("Preco.Atualizar", new Message<string, string> { Key = JsonSerializer.Serialize(new { preco.ProdutoId }), Value = JsonSerializer.Serialize(preco) });
                }
            }

            return CreatedAtAction("GetPreco", new { id = preco.PrecoId }, preco);


        }

        // DELETE: api/Precos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreco(int id)
        {
            if (_context.Preco == null)
            {
                return NotFound();
            }
            var preco = await _context.Preco.FindAsync(id);
            if (preco == null)
            {
                return NotFound();
            }

            _context.Preco.Remove(preco);
            await _context.SaveChangesAsync();

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var result = await producer.ProduceAsync("Preco.Apagar", new Message<string, string> { Key = JsonSerializer.Serialize(new { preco.ProdutoId }), Value = JsonSerializer.Serialize(preco) });
            }

            return NoContent();
        }

        private bool PrecoExists(int id)
        {
            return (_context.Preco?.Any(e => e.PrecoId == id)).GetValueOrDefault();
        }
    }
}
