using LocadoraVeiculosApi.Data;
using LocadoraVeiculosApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LocadoraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocadoraDb")));

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// ===== File: appsettings.json (add to project root) =====
{
    "ConnectionStrings": {
        "LocadoraDb": "Server=.\\SQLEXPRESS;Database=LocadoraVeiculosDB;Trusted_Connection=True;TrustServerCertificate=True;"
    },
  "Logging": {
        "LogLevel": {
            "Default": "Information"
        }
    }
}
