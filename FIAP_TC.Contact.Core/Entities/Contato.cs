namespace FIAP_TC.Contact.Core.Entities;

public class Contato
{
    public int Id { get; set; }
    public int? DDD { get; set; }
    public string? Email { get; set; }
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataModificacao { get; set; }
}