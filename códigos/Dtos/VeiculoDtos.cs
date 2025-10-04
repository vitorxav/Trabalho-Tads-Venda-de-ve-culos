using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculosApi.Dtos
{
    public class CreateVeiculoDto
    {
        [Required] public string Modelo { get; set; }
        [Range(1900, 2100)] public int AnoFabricacao { get; set; }
        [Range(0, int.MaxValue)] public int Quilometragem { get; set; }
        public bool Disponivel { get; set; } = true;
        [Required] public int FabricanteId { get; set; }
    }

    public class UpdateVeiculoDto : CreateVeiculoDto { }
}
