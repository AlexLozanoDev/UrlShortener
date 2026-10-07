using Infrastructure.Exceptions;

namespace Api.Common.Errors.ExceptionMappers
{
    public class ExternalServiceExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is ExternalServiceException;

        public ApiErrorDescriptor Map(Exception exception, HttpContext context) => new(
            StatusCodes.Status502BadGateway,
            ApiErrorCodes.ExternalServiceError,
            "Un servicio externo no respondió correctamente. Por favor, inténtelo de nuevo más tarde.",
            LogLevel: LogLevel.Error);
    }
}
