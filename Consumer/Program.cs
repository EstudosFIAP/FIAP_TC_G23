using Consumer;
using FIAP_TC.Case.Consumer.Data;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DbTcContext>(options => options.UseSqlServer(connection));

//builder.Services.AddTransient<IAtendimentoRepository, AtendimentoRepository>();

var host = builder.Build();
host.Run();
