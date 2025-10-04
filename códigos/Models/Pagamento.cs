using System;

namespace LocadoraVeiculosApi.Models
{
	public class Pagamento
	{
		public int PagamentoId { get; set; }
		public DateTime DataPagamento { get; set; }
		public decimal Valor { get; set; }
		public string Metodo { get; set; } // Ex: Cartão, Boleto, Pix

		public int AluguelId { get; set; }
		public Aluguel Aluguel { get; set; }
	}
}
