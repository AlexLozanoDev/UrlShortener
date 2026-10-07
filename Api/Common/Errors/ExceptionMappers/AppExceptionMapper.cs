using Api.Common.Responses;
using Application.Common.Exceptions;

namespace Api.Common.Errors.ExceptionMappers
{
    public class AppExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is AppException;

        public ApiErrorDescriptor Map(Exception exception, HttpContext context) => exception switch
        {
            RequestValidationException e => new(StatusCodes.Status400BadRequest, e.Code, e.Message, e.Errors.SelectMany(kv => kv.Value.Select(msg => new ApiErrorDetail(kv.Key, ApiErrorCodes.ValidationError, msg))).ToList(), LogLevel: LogLevel.Information),
            NotFoundException e => new(StatusCodes.Status404NotFound, e.Code, e.Message, null, LogLevel: LogLevel.Warning),
            AuthenticationFailedException e => new(StatusCodes.Status401Unauthorized, e.Code, e.Message, null, LogLevel: LogLevel.Warning),
            ForbiddenException e => new(StatusCodes.Status403Forbidden, e.Code, e.Message, null, LogLevel: LogLevel.Warning),
            ConflictException e => new(StatusCodes.Status409Conflict, e.Code, e.Message, null, LogLevel: LogLevel.Warning),
            _ => new(StatusCodes.Status500InternalServerError, ApiErrorCodes.InternalError, exception.Message, LogLevel: LogLevel.Error)

        };
    }
}
