using Application.Common.Errors;

namespace Application.Common.Exceptions
{
    public class AuthenticationFailedException(string message, string code = AppErrorCodes.InvalidRefreshToken): AppException(code, message)
    {
    }
}
