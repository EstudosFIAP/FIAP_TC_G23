using FIAP_TC.Case.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP_TC.Contact.Infrastructure.Data.Configurations;

public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("contatos");

        builder.HasKey(e => e.IdContato).HasName("PK__contatos__3D4FFC56A86731CB");

        builder.Property(e => e.IdContato).HasColumnName("id_contato");

        builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");

        builder.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");

        builder.Property(e => e.Telefone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("telefone");
    }
}