using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculosApi.Dtos
{
    public class CreateClienteDto
    {
        [Required] public string Nome { get; set; }
        [Required, StringLength(11, MinimumLength = 11)] public string CPF { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
    }

    public class UpdateClienteDto
    {
        [Required] public string Nome { get; set; }
        [Required, StringLength(11, MinimumLength = 11)] public string CPF { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
    }
}