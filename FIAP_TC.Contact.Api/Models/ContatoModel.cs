using FIAP_TC.Contact.Api.Extensions.CustomDataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FIAP_TC.Contact.Api.Models;

/// <summary>
///     Modelo para a criação de contato, considerando obrigatoriedade e validação dos valores dos campos.
/// </summary>
public class ContatoModel
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O campo DDD é obrigatório.")]
    [AllowedPhonePrefix(ErrorMessage = "DDD não encontrado, insira um DDD válido e tente novamente")]
    public int? DDD { get; set; }

    [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
    [CustomPhone(ErrorMessage = "Telefone inválido, informe um número válido e tente novamente.")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [CustomEmailAddress(ErrorMessage = "Email inválido, informe um endereço válido e tente novamente.")]
    public string? Email { get; set; }
}