using Application.Common.Errors;

namespace Application.Common.Exceptions
{
    public class ConflictException(string message, string code = AppErrorCodes.AliasAlreadyTaken): AppException(code, message)
    {
    }
}
