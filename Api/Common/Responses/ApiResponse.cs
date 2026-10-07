namespace Api.Common.Responses
{
    public sealed class ApiResponse<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public string? Message { get; init; }
        public ApiError? Error { get; init; }
        public string CorrelationId { get; init; } = string.Empty;
    }

    public static class ApiResponse
    {
        public static ApiResponse<T> Ok<T>(T data, string? message = null, string? correlationId = null) =>
        new() { Success = true, Data = data, Message = message, CorrelationId = correlationId ?? "" };

        public static ApiResponse<object?> Fail(ApiError error, string? correlationId = null) =>
            new() { Success = false, Error = error, CorrelationId = correlationId ?? "" };
    }
}
