using LocadoraVeiculosApi.Data;
using LocadoraVeiculosApi.Dtos;
using LocadoraVeiculosApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LocadoraVeiculosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly LocadoraContext _db;
        public VeiculosController(LocadoraContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Veiculos.Include(v => v.Fabricante).ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var v = await _db.Veiculos.Include(x => x.Fabricante).FirstOrDefaultAsync(x => x.VeiculoId == id);
            if (v == null) return NotFound();
            return Ok(v);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVeiculoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var v = new Veiculo
            {
                Modelo = dto.Modelo,
                AnoFabricacao = dto.AnoFabricacao,
                Quilometragem = dto.Quilometragem,
                Disponivel = dto.Disponivel,
                IdFabricante = dto.IdFabricante
            };
            _db.Veiculos.Add(v);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = v.VeiculoId }, v);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVeiculoDto dto)
        {
            var v = await _db.Veiculos.FindAsync(id);
            if (v == null) return NotFound();
            v.Modelo = dto.Modelo; v.AnoFabricacao = dto.AnoFabricacao; v.Quilometragem = dto.Quilometragem; v.Disponivel = dto.Disponivel; v.FabricanteId = dto.FabricanteId;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var v = await _db.Veiculos.FindAsync(id);
            if (v == null) return NotFound();
            _db.Veiculos.Remove(v);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // Filter: por fabricante (INNER JOIN)
        [HttpGet("por-fabricante")]
        public async Task<IActionResult> GetByFabricante(string nome)
        {
            var q = from v in _db.Veiculos
                    join f in _db.Fabricantes on v.IdFabricante equals f.IdFabricante
                    where f.Nome.Contains(nome)
                    select new { v.IdVeiculo, v.Modelo, v.AnoFabricacao, v.Quilometragem, Fabricante = f.Nome };
            return Ok(await q.ToListAsync());
        }

        // Filter: veiculos com ultimo aluguel (LEFT JOIN style)
        [HttpGet("com-ultimo-aluguel")]
        public async Task<IActionResult> VeiculosComUltimoAluguel()
        {
            var q = from v in _db.Veiculos
                    join a in _db.Alugueis on v.IdVeiculo equals a.VeiculoId into alugueis
                    select new
                    {
                        v.VeiculoId,
                        v.Modelo,
                        v.Disponivel,
                        UltimoAluguel = alugueis.OrderByDescending(x => x.DataInicio).FirstOrDefault()
                    };
            return Ok(await q.ToListAsync());
        }
    }
}
s