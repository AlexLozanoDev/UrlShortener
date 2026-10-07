using Domain.Common.Errors;

namespace Domain.Common.Exceptions
{
    public class DomainValidationException (string message, string code = DomainErrorCodes.ValitadionFailed) : DomainExceptions(code, message)
    {
    }
}
