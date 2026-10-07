using Application.Common.Errors;

namespace Application.Common.Exceptions
{
    public class ForbiddenException(string message, string code = AppErrorCodes.AuthenticationRequired): AppException(code, message)
    {
    }
}
