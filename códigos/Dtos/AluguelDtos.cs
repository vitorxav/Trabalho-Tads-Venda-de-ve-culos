using System;
using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculosApi.Dtos
{
    public class CreateAluguelDto
    {
        [Required] public DateTime DataInicio { get; set; }
        [Required] public DateTime DataFim { get; set; }
        [Range(0, int.MaxValue)] public int QuilometragemInicial { get; set; }
        public int? QuilometragemFinal { get; set; }
        [Required] public decimal ValorDiaria { get; set; }
        [Required] public int ClienteId { get; set; }
        [Required] public int VeiculoId { get; set; }
        [Required] public int FuncionarioId { get; set; }
    }

    public class UpdateAluguelDto : CreateAluguelDto { }
}