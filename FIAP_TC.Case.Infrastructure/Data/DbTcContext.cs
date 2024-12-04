using FIAP_TC.Case.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FIAP_TC.Case.Infrastructure.Data
{
    public partial class DbTcContext : DbContext
    {
        public DbTcContext(DbContextOptions<DbTcContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Atendimento> Atendimentos { get; set; }
        public virtual DbSet<Contato> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica as configurações das entidades a partir do Assembly atual
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
