namespace LocadoraVeiculosApi.Models
{
    public class Aluguel
    {
        public int AluguelId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int QuilometragemInicial { get; set; }
        public int? QuilometragemFinal { get; set; }
        public decimal ValorDiaria { get; set; }
        public decimal ValorTotal { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }

        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        public Pagamento Pagamento { get; set; }
    }

}
