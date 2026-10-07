using Api.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ActionResult<ApiResponse<T>> ApiOk<T>(
            this ControllerBase controller,
            T data,
            string? message = null)
            => controller.Ok(
                ApiResponse.Ok(data, message, GetCorrelationId(controller)));

        public static ActionResult<ApiResponse<T>> ApiCreated<T>(
            this ControllerBase controller,
            T data,
            string? message = null)
            => controller.StatusCode(
                StatusCodes.Status201Created,
                ApiResponse.Ok(data, message, GetCorrelationId(controller)));

        private static string GetCorrelationId(ControllerBase controller)
        => Activity.Current?.TraceId.ToString() ?? controller.HttpContext.TraceIdentifier;
    }
}
