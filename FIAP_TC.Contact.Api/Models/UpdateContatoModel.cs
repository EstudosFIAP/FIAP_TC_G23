using FIAP_TC.Contact.Api.Extensions.CustomDataAnnotations;

namespace FIAP_TC.Contact.Api.Models;

/// <summary>
///     Modelo para a atualização de contato, considerando validação dos valores dos campos.
/// </summary>
public class UpdateContatoModel
{
    public string? Nome { get; set; }

    [AllowedPhonePrefix(ErrorMessage = "DDD não encontrado, insira um DDD válido e tente novamente")]
    public int? DDD { get; set; }

    [CustomPhone(ErrorMessage = "Telefone inválido, informe um número válido e tente novamente.")]
    public string? Telefone { get; set; }

    [CustomEmailAddress(ErrorMessage = "Email inválido, informe um endereço válido e tente novamente.")]
    public string? Email { get; set; }
}