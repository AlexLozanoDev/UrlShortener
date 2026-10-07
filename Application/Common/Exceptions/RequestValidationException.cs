using Application.Common.Errors;

namespace Application.Common.Exceptions
{
    public class RequestValidationException(
        IReadOnlyDictionary<string, string[]> errors, 
        string message = "Uno o mas errores de validacion ocurrieron") : AppException(AppErrorCodes.ValidationFailed, message)
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
    }
}
