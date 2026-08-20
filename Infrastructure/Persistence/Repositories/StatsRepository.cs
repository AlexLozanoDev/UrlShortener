using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public class StatsRepository(ApplicationDbContext context) : IStatsRepository
    {
        public async Task AddAsync(LinkClickStats stats, CancellationToken cancellationToken = default)
        {
            await context.LinkClickStats.AddAsync(stats, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
