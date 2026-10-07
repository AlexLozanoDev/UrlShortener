using Api.Common.Responses;
using System.Diagnostics;

namespace Api.Extensions
{
    internal static class HttpContextExtensions
    {
        public static Task WriteApiErrorAsync(
            this HttpContext context,
            int statusCode, 
            string code,
            string message,
            CancellationToken cancellationToken = default)
        {
            context.Response.StatusCode = statusCode;

            var payload = ApiResponse.Fail(
                new ApiError(code, message),
                Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier);

            return context.Response.WriteAsJsonAsync(payload, cancellationToken);
        }
    }
}
