using FIAP_TC.Contact.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FIAP_TC.Contact.Infrastructure.Data;

public partial class DbTcFase1Context : DbContext
{
    public DbTcFase1Context(DbContextOptions<DbTcFase1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Contato> Contatos { get; set; }
    public virtual DbSet<Estado> Estados { get; set; }
    public virtual DbSet<Municipio> Municipios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}