namespace Domain.Common.Exceptions
{
    public class DomainConflictException(string code, string message) : DomainExceptions(code, message)
    {
    }
}
