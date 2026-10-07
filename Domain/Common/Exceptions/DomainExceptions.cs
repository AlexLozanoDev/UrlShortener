namespace Domain.Common.Exceptions
{
    public abstract class DomainExceptions (string code, string message) : Exception(message)
    {
        public string Code { get; } = code;

    }
}
