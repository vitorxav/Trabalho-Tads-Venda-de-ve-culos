namespace LocadoraVeiculosApi.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }

        public ICollection<Aluguel> Alugueis { get; set; }
    }

}
