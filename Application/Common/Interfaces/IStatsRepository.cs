using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IStatsRepository
    {
        Task AddAsync(LinkClickStats stats, CancellationToken cancellationToken = default);
    }
}
