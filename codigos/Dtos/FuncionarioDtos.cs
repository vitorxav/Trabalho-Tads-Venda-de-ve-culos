using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculosApi.Dtos
{
	public class CreateFuncionarioDto
	{
		[Required] public string Nome { get; set; }
		[Required] public string Cargo { get; set; }
		[Required, EmailAddress] public string Email { get; set; }
	}

	public class UpdateFuncionarioDto : CreateFuncionarioDto { }
}
