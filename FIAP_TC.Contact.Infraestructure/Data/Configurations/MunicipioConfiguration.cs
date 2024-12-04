using FIAP_TC.Contact.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP_TC.Contact.Infrastructure.Data.Configurations;

public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
{
    public void Configure(EntityTypeBuilder<Municipio> builder)
    {
        builder.ToTable("municipios");

        builder.HasKey(e => e.CodigoIbge).HasName("PK__municipi__B2A901A3DCB7C4D6");

        builder.HasIndex(e => e.SiafiId, "UQ__municipi__D22B6EE7F0D72C7D").IsUnique();

        builder.Property(e => e.CodigoIbge)
            .ValueGeneratedNever()
            .HasColumnName("codigo_ibge");

        builder.Property(e => e.Capital).HasColumnName("capital");

        builder.Property(e => e.CodigoUf).HasColumnName("codigo_uf");

        builder.Property(e => e.Ddd).HasColumnName("ddd");

        builder.Property(e => e.FusoHorario)
            .HasMaxLength(32)
            .IsUnicode(false)
            .HasColumnName("fuso_horario");
        builder.Property(e => e.Latitude).HasColumnName("latitude");

        builder.Property(e => e.Longitude).HasColumnName("longitude");

        builder.Property(e => e.Nome)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("nome");

        builder.Property(e => e.SiafiId)
            .HasMaxLength(4)
            .IsUnicode(false)
            .HasColumnName("siafi_id");

        builder.HasOne(d => d.CodigoUfNavigation).WithMany(p => p.Municipios)
            .HasForeignKey(d => d.CodigoUf)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__municipio__codig__03F0984C");
    }
}