﻿namespace LocadoraVeiculosApi.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }

        public ICollection<Aluguel> Alugueis { get; set; }
    }

}
