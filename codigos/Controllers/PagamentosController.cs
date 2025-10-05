using LocadoraVeiculosApi.Data;
using LocadoraVeiculosApi.Dtos;
using LocadoraVeiculosApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LocadoraVeiculosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentosController : ControllerBase
    {
        private readonly LocadoraContext _db;
        public PagamentosController(LocadoraContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Pagamentos.Include(p => p.Aluguel).ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _db.Pagamentos.Include(x => x.Aluguel).FirstOrDefaultAsync(x => x.PagamentoId == id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePagamentoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var aluguel = await _db.Alugueis.FindAsync(dto.AluguelId);
            if (aluguel == null) return BadRequest("Aluguel não encontrado");

            var pag = new Pagamento { DataPagamento = dto.DataPagamento, Valor = dto.Valor, Metodo = dto.Metodo, AluguelId = dto.AluguelId };
            _db.Pagamentos.Add(pag);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = pag.PagamentoId }, pag);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePagamentoDto dto)
        {
            var p = await _db.Pagamentos.FindAsync(id);
            if (p == null) return NotFound();
            p.DataPagamento = dto.DataPagamento; p.Valor = dto.Valor; p.Metodo = dto.Metodo;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Pagamentos.FindAsync(id);
            if (p == null) return NotFound();
            _db.Pagamentos.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}