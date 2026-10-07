using System.Text.Json;

namespace Api.Common.Errors.ExceptionMappers
{
    public class FrameworkExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) =>
        exception is BadHttpRequestException
            or JsonException
            or FormatException;

        public ApiErrorDescriptor Map(Exception exception, HttpContext context) =>
            exception switch
            {
                BadHttpRequestException => new ApiErrorDescriptor(
                    StatusCode: StatusCodes.Status400BadRequest,
                    Code: ApiErrorCodes.InvalidRequestBody,
                    Message: "El cuerpo de la solicitud no tiene un formato válido.",
                    LogLevel: LogLevel.Warning),

                JsonException => new ApiErrorDescriptor(
                    StatusCode: StatusCodes.Status400BadRequest,
                    Code: ApiErrorCodes.InvalidJson,
                    Message: "El JSON enviado no tiene un formato válido.",
                    LogLevel: LogLevel.Warning),

                _ => new ApiErrorDescriptor(
                    StatusCode: StatusCodes.Status400BadRequest,
                    Code: ApiErrorCodes.InvalidRequest,
                    Message: "La solicitud no tiene un formato válido.",
                    LogLevel: LogLevel.Warning)
            };
    }
}
