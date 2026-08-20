using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common.Interfaces

{
    public interface ILinkRepository
    {
        Task<Link> GetByShortCodeAsync (ShortCode shortCode, CancellationToken cancellationToken = default);
        Task AddAsync(Link link, CancellationToken cancellationToken);
    }
}
