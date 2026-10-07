using Api.Common.Responses;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace Api.Common.Errors
{
    public sealed class GlobalExceptionHandler(
        IEnumerable<IExceptionMapper> mappers,
        IHostEnvironment environment,
        ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
            {
                logger.LogInformation("Peticion cancelada por el cliente, {traceId}", httpContext.TraceIdentifier);
                return true;
            }

            var mapper = mappers.FirstOrDefault(m => m.CanMap(exception));

            var descriptor = mapper is not null
                ? mapper.Map(exception, httpContext)
                : new ApiErrorDescriptor(
                    StatusCodes.Status500InternalServerError,
                    ApiErrorCodes.InternalError,
                    environment.IsDevelopment() ? exception.Message : "Ocurrio un error inesperado. Intentalo mas tarde.",
                    LogLevel: LogLevel.Error);


            logger.Log(descriptor.LogLevel, exception, "[{Code}] {traceId}: {Message}", descriptor.Code, httpContext.TraceIdentifier, exception.Message);

            if (httpContext.Response.HasStarted) return false;
            
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = descriptor.StatusCode;
            httpContext.Response.ContentType = "application/json";

            var payload = ApiResponse.Fail(
                new ApiError(
                    descriptor.Code,
                    descriptor.Message,
                    descriptor.Details
                ),
                Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier);

            await httpContext.Response.WriteAsJsonAsync(payload, cancellationToken);

            return true;
        }
    }
}
