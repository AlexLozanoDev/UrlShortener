using Domain.Common.Exceptions;

namespace Domain.ValueObjects;

public sealed record ShortCode
{
    public string Value { get; }

    public ShortCode (string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException("El código corto no puede estar vacío.");
        }

        if (IsValidBase64(value))
        {
            throw new DomainValidationException("El código corto contiene caracteres inválidos para base62.");
        }

        Value = value;
    }

    private static bool IsValidBase64(string value)
    {
        foreach (char c in value)
        {
            if (char.IsLetterOrDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    public override string ToString() => Value;

}
