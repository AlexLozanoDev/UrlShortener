using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common.Interfaces

{
    public interface ILinkRepository
    {
        Task<Link> GetByShortCodeAsync (ShortCode shortCode, CancellationToken cancellationToken = default);
        Task AddAsync(Link link, CancellationToken cancellationToken);
        Task UpdateAsync(Link link, CancellationToken cancellationToken);
        Task<(IEnumerable<Link> Links, int TotalCount)> GetByOwnerIdPagedAsync(Guid ownerId, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
