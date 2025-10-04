using LocadoraVeiculosApi.Data;
using LocadoraVeiculosApi.Dtos;
using LocadoraVeiculosApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocadoraVeiculosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlugueisController : ControllerBase
    {
        private readonly LocadoraContext _db;
        public AlugueisController(LocadoraContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Alugueis.Include(a => a.Cliente).Include(a => a.Veiculo).Include(a => a.Funcionario).ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var a = await _db.Alugueis.Include(x => x.Cliente).Include(x => x.Veiculo).Include(x => x.Funcionario).FirstOrDefaultAsync(x => x.AluguelId == id);
            if (a == null) return NotFound();
            return Ok(a);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAluguelDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ve = await _db.Veiculos.FindAsync(dto.IdVeiculo);
            if (ve == null) return BadRequest("Veiculo não encontrado");
            if (!ve.Disponivel) return BadRequest("Veiculo não disponível");

            var cliente = await _db.Clientes.FindAsync(dto.IdCliente);
            if (cliente == null) return BadRequest("Cliente não encontrado");

            var funcionario = await _db.Funcionarios.FindAsync(dto.IdFuncionario);
            if (funcionario == null) return BadRequest("Funcionário não encontrado");

            var aluguel = new Aluguel
            {
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                QuilometragemInicial = dto.QuilometragemInicial,
                QuilometragemFinal = dto.QuilometragemFinal,
                ValorDiaria = dto.ValorDiaria,
                ClienteId = dto.ClienteId,
                VeiculoId = dto.VeiculoId,
                FuncionarioId = dto.FuncionarioId
            };

            var dias = (int)Math.Ceiling((aluguel.DataFim - aluguel.DataInicio)?.TotalDays ?? 1);
            aluguel.ValorTotal = dias * aluguel.ValorDiaria;

            ve.Disponivel = false;

            _db.Alugueis.Add(aluguel);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = aluguel.AluguelId }, aluguel);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAluguelDto dto)
        {
            var a = await _db.Alugueis.FindAsync(id);
            if (a == null) return NotFound();

            a.DataInicio = dto.DataInicio;
            a.DataFim = dto.DataFim;
            a.QuilometragemInicial = dto.QuilometragemInicial;
            a.QuilometragemFinal = dto.QuilometragemFinal;
            a.ValorDiaria = dto.ValorDiaria;
            a.ClienteId = dto.ClienteId;
            a.VeiculoId = dto.VeiculoId;
            a.FuncionarioId = dto.FuncionarioId;

            var dias = (int)Math.Ceiling((a.DataFim - a.DataInicio)?.TotalDays ?? 1);
            a.ValorTotal = dias * a.ValorDiaria;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Alugueis.FindAsync(id);
            if (a == null) return NotFound();

            // ao deletar um aluguel, liberar veículo
            var ve = await _db.Veiculos.FindAsync(a.IdVeiculo);
            if (ve != null) ve.Disponivel = true;

            _db.Alugueis.Remove(a);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}