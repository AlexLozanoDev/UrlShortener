using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common.Interfaces
{
    public interface IStatsRepository
    {
        Task AddAsync(LinkClickStats stats, CancellationToken cancellationToken = default);
        Task<IEnumerable<LinkClickStats>> GetByLinkIdAsync(ShortCode shortCode, CancellationToken cancelationToken = default);
    }
}
