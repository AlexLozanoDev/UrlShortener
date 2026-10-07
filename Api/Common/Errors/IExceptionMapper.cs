namespace Api.Common.Errors
{
    public interface IExceptionMapper
    {
        bool CanMap(Exception exception);
        ApiErrorDescriptor Map(Exception exception, HttpContext context);
    }
}
