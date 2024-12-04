namespace FIAP_TC.Case.Api.Models
{
    public class AtendimentoModel
    {
        public required string Assunto { get; set; }
        public required string Descricao { get; set; }
        public int IdContato { get; set; }
    }
}
