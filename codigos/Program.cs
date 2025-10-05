using LocadoraVeiculosApi.Data;
using LocadoraVeiculosApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ------------------ Serviços ------------------
builder.Services.AddControllers();

// Swagger com documentação XML
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Inclui os comentários XML (gerados no build do projeto)
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "LocadoraVeiculos API",
        Version = "v1",
        Description = "API para gerenciar locadora de veículos - CRUD e relatórios"
    });
});

// Configuração do banco de dados
builder.Services.AddDbContext<LocadoraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocadoraDb")));

// ------------------ Build da aplicação ------------------
var app = builder.Build();

// Middleware de tratamento de erros customizado
app.UseMiddleware<ErrorHandlerMiddleware>();

// ------------------ Pipeline ------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LocadoraVeiculos API v1");
        c.RoutePrefix = string.Empty; // Exibe o Swagger na raiz (http://localhost:5000)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
