using FIAP_TC.Contact.Api.Repository.Interfaces;
using FIAP_TC.Contact.Api.Repository;
using FIAP_TC.Contact.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using FIAP_TC.Contact.Infraestructure.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbTcFase1Context>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
//builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();

builder.Services.UseHttpClientMetrics();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMetricServer();
app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllers();

// health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.MapGet("/readiness", async (IServiceProvider services) =>
{
    // Verifique o banco de dados
    var dbContext = services.GetService<DbTcFase1Context>();
    if (dbContext == null)
    {
        return Results.Json(new { status = "Not Ready - DB context is null" }, statusCode: 503);
    }
    bool dbReady = await dbContext.Database.CanConnectAsync();

    // Verifique o RabbitMQ
    var rabbitMQService = services.GetService<IRabbitMQService>();
    if (rabbitMQService == null)
    {
        return Results.Json(new { status = "Not Ready - RabbitMQ service is null" }, statusCode: 503);
    }
    bool rabbitMQReady = rabbitMQService.CheckConnection();

    if (dbReady/* && rabbitMQReady*/)
        return Results.Ok(new { status = "Ready" });

    return Results.Json(new { status = "Not Ready" }, statusCode: 503);
});

app.Run();
