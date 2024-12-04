using System.ComponentModel.DataAnnotations;

namespace FIAP_TC.Contact.Api.Extensions.CustomDataAnnotations;

/// <summary>
///     Atributo de validação telefone
/// </summary>
public class CustomPhone : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if (!(value is string valueAsString))
            return false;

        bool tel = valueAsString.All(char.IsDigit);

        if (tel == false)
            return false;

        var telephone = new string(valueAsString.Where(char.IsDigit).ToArray());

        if (telephone.Length < 8)
            return false;
        else if (telephone.Length > 9)
            return false;
        else
            return true;
    }
}