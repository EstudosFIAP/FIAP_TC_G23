namespace FIAP_TC.Contact.Core.DTO;

public class ContatoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public int? DDD { get; set; } // Allow null values for DDD
    public DateTime DataCriacao { get; set; }
    public DateTime DataModificacao { get; set; }
    public string NomeEstado { get; set; } = string.Empty;
    public string Regiao { get; set; } = string.Empty;
}