using Application.Common.Errors;

namespace Application.Common.Exceptions
{
    public class NotFoundException(string message, string code = AppErrorCodes.LinkNotFound): AppException(code, message)
    {
    }
}
