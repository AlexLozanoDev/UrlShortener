namespace Application.Links.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base (message) { }
        public NotFoundException(string name, object key) : base($"El recurso '{name}' con el valor '{key}' no fue encontrado") { }
    }
}
