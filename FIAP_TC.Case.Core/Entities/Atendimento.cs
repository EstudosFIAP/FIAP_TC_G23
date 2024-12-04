namespace FIAP_TC.Case.Core.Entities
{
    public class Atendimento
    {
        public int IdAtendimento { get; set; }
        public required string Assunto { get; set; } = string.Empty;
        public required string Descricao { get; set; } = string.Empty;
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataModificacao { get; set; }
        public int IdContato { get; set; }
        public Contato? Contato { get; set; }
    }
}
