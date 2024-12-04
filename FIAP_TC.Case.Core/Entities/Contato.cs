namespace FIAP_TC.Case.Core.Entities;

public class Contato
{
    public int IdContato { get; set; }
    public int? DDD { get; set; }
    public required string Email { get; set; }
    public required string Nome { get; set; }
    public string? Telefone { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataModificacao { get; set; }
}