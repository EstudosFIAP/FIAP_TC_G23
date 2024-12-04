namespace FIAP_TC.Case.Core.DTO
{
    public class AtendimentoDTO
    {
        public int IdAtendimento { get; set; }
        public string Assunto { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataModificacao { get; set; }
        public int IdContato { get; set; }
        public string NomeContato { get; set; } = string.Empty;
        public string EmailContato { get; set; } = string.Empty;
    }
}


