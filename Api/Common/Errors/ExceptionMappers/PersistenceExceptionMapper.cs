using Infrastructure.Common;
using Infrastructure.Exceptions;

namespace Api.Common.Errors.ExceptionMappers
{
    public class PersistenceExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is PersistenceExceptions;

        public ApiErrorDescriptor Map(Exception exception, HttpContext context)
        {
            var persistenceException = exception as PersistenceExceptions;

            return persistenceException.code switch
            {
                PersistenceErrorCodes.UniqueViolation => new ApiErrorDescriptor
                (
                    StatusCodes.Status409Conflict,
                    ApiErrorCodes.ResourceAlreadyExists,
                    "El recurso ya existe",
                    LogLevel: LogLevel.Warning
                ),
                PersistenceErrorCodes.ForeignKeyViolation => new ApiErrorDescriptor
                (
                    StatusCodes.Status409Conflict,
                    ApiErrorCodes.ResourceInUse,
                    "El recurso está en uso",
                    LogLevel: LogLevel.Warning
                ),
                PersistenceErrorCodes.InvalidData => new ApiErrorDescriptor
                (
                    StatusCodes.Status400BadRequest,
                    ApiErrorCodes.InvalidData,
                    "Los datos proporcionados no son válidos",
                    LogLevel: LogLevel.Warning
                ),
                _ => new ApiErrorDescriptor
                (
                    StatusCodes.Status500InternalServerError,
                    ApiErrorCodes.PersistenceFailed,
                    "No fue posible completar la operación",
                    LogLevel: LogLevel.Error
                )
            };
        }
    }
}
