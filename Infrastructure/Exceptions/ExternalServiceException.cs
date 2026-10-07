namespace Infrastructure.Exceptions
{
    public class ExternalServiceException(string message, Exception inner): Exception(message, inner)
    {
        public string Code { get; } = "EXTERNAL_SERVICE_ERROR";
    }
}
