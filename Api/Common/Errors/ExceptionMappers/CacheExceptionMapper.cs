using Infrastructure.Exceptions;


//Deprecated
namespace Api.Common.Errors.ExceptionMappers
{
    public class CacheExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is CacheUnvailableException;

        public ApiErrorDescriptor Map(Exception exception, HttpContext context) => new(
            StatusCodes.Status503ServiceUnavailable,
            ApiErrorCodes.CacheUnavailable,
            "El servicio no esta disponible temporalmente",
            LogLevel: LogLevel.Error);
    }
}
