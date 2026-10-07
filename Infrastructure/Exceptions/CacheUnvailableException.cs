namespace Infrastructure.Exceptions
{
    public class CacheUnvailableException(string message, Exception inner): Exception(message, inner)
    {
        public string Code { get; } = "CACHE_UNAVAILABLE";
    }
}
