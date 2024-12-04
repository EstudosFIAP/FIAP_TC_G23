namespace FIAP_TC.Case.Consumer.Entities;

public class Contato
{
    public int Id { get; set; }
    public int? DDD { get; set; }
    public required string Email { get; set; }
    public required string Nome { get; set; }
    public string? Telefone { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataModificacao { get; set; }
}