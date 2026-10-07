using Domain.Common.Exceptions;

namespace Api.Common.Errors.ExceptionMappers
{
    public class DomainExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is DomainExceptions;

        public ApiErrorDescriptor Map(Exception exception, HttpContext context) => exception switch
        {
            DomainValidationException e => new(StatusCodes.Status400BadRequest, e.Code, e.Message, LogLevel: LogLevel.Warning),
            DomainConflictException e => new(StatusCodes.Status409Conflict, e.Code, e.Message, LogLevel: LogLevel.Warning),
            _ => new(StatusCodes.Status400BadRequest, "DOMAIN_ERROR", exception.Message)
        };
    }
}
