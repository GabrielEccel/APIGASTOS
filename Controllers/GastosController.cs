using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIGastos.Models;
using APIGastos.Data;

namespace APIGastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GastosController : ControllerBase
    {
        private readonly Contexto _context;

        public GastosController(Contexto context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gasto>>> GetGastos()
        {
            return await _context.Gastos.ToListAsync();
        }

        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetTotalGastos()
        {
            return await _context.Gastos.SumAsync(g => g.Valor);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gasto>> GetGasto(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);

            if (gasto == null)
            {
                return NotFound();
            }
                
            return gasto;
        }

        [HttpPost]
        public async Task<ActionResult<Gasto>> PostGasto([FromBody] Gasto gasto)
        {
            if (gasto.Id != 0)
            {
                return BadRequest("Novo gasto não deve conter ID. Remova a propriedade Id ou defina como 0.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGasto), new { id = gasto.Id }, gasto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGasto(int id, [FromBody] Gasto gasto)
        {
            if (id != gasto.Id)
            {
                return BadRequest("ID na URL não corresponde ao ID no objeto");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(gasto).State = EntityState.Modified;

            await _context.SaveChangesAsync();  

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGasto(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null)
            {
                return NotFound();
            }

            _context.Gastos.Remove(gasto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.Id == id);
        }
    }
}