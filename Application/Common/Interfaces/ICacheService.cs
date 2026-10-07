namespace Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string key, CancellationToken cancellationToken = default);
        Task SetAsync(string key, string value, TimeSpan absoluteExperationRelativeToNow, CancellationToken cancellationToken = default);
    }
}
