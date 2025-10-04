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
    public class FuncionariosController : ControllerBase
    {
        private readonly LocadoraContext _db;
        public FuncionariosController(LocadoraContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Funcionarios.ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var f = await _db.Funcionarios.FindAsync(id);
            if (f == null) return NotFound();
            return Ok(f);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFuncionarioDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var f = new Funcionario { Nome = dto.Nome, Cargo = dto.Cargo, Email = dto.Email };
            _db.Funcionarios.Add(f);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = f.FuncionarioId }, f);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFuncionarioDto dto)
        {
            var f = await _db.Funcionarios.FindAsync(id);
            if (f == null) return NotFound();
            f.Nome = dto.Nome; f.Cargo = dto.Cargo; f.Email = dto.Email;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var f = await _db.Funcionarios.FindAsync(id);
            if (f == null) return NotFound();
            _db.Funcionarios.Remove(f);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}