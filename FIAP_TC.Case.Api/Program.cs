using FIAP_TC.Case.Api.Repository.Interfaces;
using FIAP_TC.Case.Api.Repository;
using FIAP_TC.Case.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Prometheus; // Import necessário para o Prometheus

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona a string de conexão com o banco de dados
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbTcContext>(options => options.UseSqlServer(connection));

// Registra o IAtendimentoRepository e AtendimentoRepository no container de DI
builder.Services.AddScoped<IAtendimentoRepository, AtendimentoRepository>();

// Adiciona o Prometheus para métricas de requisições HTTP
builder.Services.UseHttpClientMetrics();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configura o Prometheus para expor métricas em /metrics
app.UseMetricServer();   // Exporá as métricas no caminho padrão /metrics
app.UseHttpMetrics();    // Coleta métricas de requisições HTTP

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Adiciona o endpoint de liveness probe
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

// Adiciona o endpoint de readiness probe
app.MapGet("/readiness", async (IServiceProvider services) =>
{
    var dbContext = services.GetService<DbTcContext>();
    if (dbContext == null)
    {
        return Results.Json(new { status = "Not Ready - DB context is null" }, statusCode: 503);
    }
    bool dbReady = await dbContext.Database.CanConnectAsync();


    if (dbReady)
    {
        return Results.Ok(new { status = "Ready" });
    }
    return Results.Json(new { status = "Not Ready" }, statusCode: 503);
});


app.Run();
