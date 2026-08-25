using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class LinkRepository(ApplicationDbContext dbContext) : ILinkRepository
    {

        public async Task AddAsync(Link link, CancellationToken cancellationToken)
        {
            await dbContext.Links.AddAsync(link, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Link> GetByShortCodeAsync(ShortCode shortCode, CancellationToken cancellationToken = default)
        {
            return await dbContext.Links
                .FirstOrDefaultAsync(l => l.ShortCode == shortCode, cancellationToken);
        }

        public async Task UpdateAsync(Link link, CancellationToken cancellationToken)
        {
            dbContext.Links.Update(link);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
