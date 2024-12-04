using FIAP_TC.Case.Consumer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP_TC.Case.Consumer.Data.Configurations
{
    public class AtendimentoConfiguration : IEntityTypeConfiguration<Atendimento>
    {
        public void Configure(EntityTypeBuilder<Atendimento> builder)
        {
            // Define o nome da tabela
            builder.ToTable("atendimentos");

            // Define a chave primária
            builder.HasKey(e => e.IdAtendimento).HasName("PK__atendime__585CB0952997AD9B");

            // Define as propriedades e colunas
            builder.Property(e => e.IdAtendimento)
                .ValueGeneratedOnAdd()  // IDENTITY(1,1) no SQL
                .HasColumnName("id_atendimento");

            builder.Property(e => e.Assunto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("assunto");

            builder.Property(e => e.Descricao)
                .IsUnicode(false)
                .HasColumnName("descricao");

            builder.Property(e => e.DataModificacao)
                .HasColumnType("datetime")
                .HasColumnName("DataModificacao");

            builder.Property(e => e.DataSolicitacao)
                .HasColumnType("datetime")
                .HasColumnName("DataSolicitacao");

            builder.Property(e => e.IdContato)
                .HasColumnName("id_contato");

            builder.HasOne(a => a.Contato)
                .WithOne()
                .HasForeignKey<Atendimento>(a => a.IdContato)
                .HasConstraintName("fk_contato_atendimento");


        }
    }
}
