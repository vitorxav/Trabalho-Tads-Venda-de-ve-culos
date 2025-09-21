using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LocadoraVeiculosApi.Data
{
    public class LocadoraContextFactory : IDesignTimeDbContextFactory<LocadoraContext>
    {
        public LocadoraContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocadoraContext>();
            // coloque aqui a connection string de desenvolvimento (temporária)
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=LocadoraVeiculosDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new LocadoraContext(optionsBuilder.Options);
        }
    }
}
