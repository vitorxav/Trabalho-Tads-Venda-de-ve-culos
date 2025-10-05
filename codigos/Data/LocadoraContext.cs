namespace LocadoraVeiculosApi.Data
{
    using LocadoraVeiculosApi.Models;
    using Microsoft.EntityFrameworkCore;

    public class LocadoraContext : DbContext
    {
        public LocadoraContext(DbContextOptions<LocadoraContext> options) : base(options) { }

        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Aluguel> Alugueis { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CPF)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Email)
                .IsUnique();

            modelBuilder.Entity<Veiculo>()
                .HasOne(v => v.Fabricante)
                .WithMany(f => f.Veiculos)
                .HasForeignKey(v => v.FabricanteId);

            modelBuilder.Entity<Aluguel>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Alugueis)
                .HasForeignKey(a => a.ClienteId);

            modelBuilder.Entity<Aluguel>()
                .HasOne(a => a.Veiculo)
                .WithMany(v => v.Alugueis)
                .HasForeignKey(a => a.VeiculoId);

            modelBuilder.Entity<Aluguel>()
                .HasOne(a => a.Funcionario)
                .WithMany(f => f.Alugueis)
                .HasForeignKey(a => a.FuncionarioId);

            modelBuilder.Entity<Pagamento>()
                .HasOne(p => p.Aluguel)
                .WithOne(a => a.Pagamento)
                .HasForeignKey<Pagamento>(p => p.AluguelId)
                .OnDelete(DeleteBehavior.Cascade);
                



            base.OnModelCreating(modelBuilder);
        }
    }
    }

