using System;
using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculosApi.Dtos
{
    public class CreatePagamentoDto
    {
        [Required] public DateTime DataPagamento { get; set; }
        [Required] public decimal Valor { get; set; }
        [Required] public string Metodo { get; set; }
        [Required] public int AluguelId { get; set; }
    }

    public class UpdatePagamentoDto : CreatePagamentoDto { }
}
