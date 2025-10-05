using LocadoraVeiculosApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LocadoraVeiculosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly LocadoraContext _db;
        public RelatoriosController(LocadoraContext db) => _db = db;

        // 1) Clientes com número de aluguéis (LEFT JOIN style)
        [HttpGet("clientes-com-alugueis")]
        public async Task<IActionResult> ClientesComAlugueis()
        {
            var q = from c in _db.Clientes
                    join a in _db.Alugueis on c.ClienteId equals a.ClienteId into alugueis
                    select new { c.ClienteId, c.Nome, TotalAlugueis = alugueis.Count() };
            return Ok(await q.ToListAsync());
        }

        // 2) Aluguéis com pagamento (INNER JOIN)
        [HttpGet("alugueis-com-pagamento")]
        public async Task<IActionResult> AlugueisComPagamento()
        {
            var q = from a in _db.Alugueis
                    join p in _db.Pagamentos on a.AluguelId equals p.AluguelId
                    select new { a.AluguelId, a.DataInicio, a.DataFim, p.Valor, p.DataPagamento };
            return Ok(await q.ToListAsync());
        }

        // 3) Faturamento por fabricante (multi-join + group)
        [HttpGet("faturamento-por-fabricante")]
        public async Task<IActionResult> FaturamentoPorFabricante()
        {
            var q = from f in _db.Fabricantes
                    join v in _db.Veiculos on f.FabricanteId equals v.FabricanteId
                    join a in _db.Alugueis on v.VeiculoId equals a.VeiculoId
                    join p in _db.Pagamentos on a.AluguelId equals p.AluguelId
                    group p by f.Nome into g
                    select new { Fabricante = g.Key, TotalFaturado = g.Sum(x => x.Valor) };
            return Ok(await q.ToListAsync());
        }

        // 4) Veículos por fabricante (INNER JOIN) - duplicate of earlier filter but here as report
        [HttpGet("veiculos-por-fabricante")]
        public async Task<IActionResult> VeiculosPorFabricante(string nome)
        {
            var q = from v in _db.Veiculos
                    join f in _db.Fabricantes on v.FabricanteId equals f.FabricanteId
                    where f.Nome.Contains(nome)
                    select new { v.VeiculoId, v.Modelo, v.AnoFabricacao, v.Quilometragem, Fabricante = f.Nome };
            return Ok(await q.ToListAsync());
        }

        // 5) Veículos com último aluguel (LEFT JOIN style)
        [HttpGet("veiculos-com-ultimo-aluguel")]
        public async Task<IActionResult> VeiculosComUltimoAluguel()
        {
            var q = from v in _db.Veiculos
                    join a in _db.Alugueis on v.VeiculoId equals a.VeiculoId into alugueis
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