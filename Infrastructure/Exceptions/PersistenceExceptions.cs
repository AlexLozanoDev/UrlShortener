namespace Infrastructure.Exceptions
{
    public sealed class PersistenceExceptions(string code, string message, Exception inner) : Exception(message, inner)
    {
        public string code { get; } = code;
    }
}
