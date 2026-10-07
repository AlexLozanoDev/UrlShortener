using Api.Common.Responses;

namespace Api.Common.Errors
{
    public sealed record ApiErrorDescriptor(
        int StatusCode,
        string Code,
        string Message,
        IReadOnlyList<ApiErrorDetail>? Details = null,
        LogLevel LogLevel = LogLevel.Error,
        string? LogExceptionMessage = null);
}
