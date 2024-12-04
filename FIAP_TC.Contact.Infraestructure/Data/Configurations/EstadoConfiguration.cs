using FIAP_TC.Contact.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP_TC.Contact.Infrastructure.Data.Configurations;

public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.ToTable("estados");

        builder.HasKey(e => e.CodigoUf).HasName("PK__estados__75B108EDB1E6A8AA");

        builder.Property(e => e.CodigoUf)
                .ValueGeneratedNever()
                .HasColumnName("codigo_uf");

        builder.Property(e => e.Latitude).HasColumnName("latitude");

        builder.Property(e => e.Longitude).HasColumnName("longitude");

        builder.Property(e => e.Nome)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("nome");

        builder.Property(e => e.Regiao)
            .HasMaxLength(12)
            .IsUnicode(false)
            .HasColumnName("regiao");

        builder.Property(e => e.Uf)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("uf");
    }
}
