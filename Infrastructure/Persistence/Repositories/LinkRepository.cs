using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Common;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Persistence.Repositories
{
    public class LinkRepository(ApplicationDbContext dbContext) : ILinkRepository
    {

        public async Task AddAsync(Link link, CancellationToken cancellationToken)
        {
            await dbContext.Links.AddAsync(link, cancellationToken);
            try { await dbContext.SaveChangesAsync(cancellationToken); }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg)
            {
                throw new PersistenceExceptions(pg.SqlState switch
                {
                    PostgresErrorCodes.UniqueViolation => PersistenceErrorCodes.UniqueViolation, // 23505 → 409
                    PostgresErrorCodes.ForeignKeyViolation => PersistenceErrorCodes.ForeignKeyViolation, // 23503 → 409
                    PostgresErrorCodes.NotNullViolation or PostgresErrorCodes.InvalidTextRepresentation => PersistenceErrorCodes.InvalidData,  // 23502/22P02 → 400
                    _ => PersistenceErrorCodes.PersistenceFailed // → 500
                }, "Error al persistir la operacion.", ex);
            }
        }

        public async Task<(IEnumerable<Link> Links, int TotalCount)> GetByOwnerIdPagedAsync(Guid ownerId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Links.Where(l => l.OwnerId == ownerId);

            int totalCount = await query.CountAsync(cancellationToken);

            var links = await query
                .OrderByDescending(l => l.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (links, totalCount);
        }

        public async Task<Link> GetByShortCodeAsync(ShortCode shortCode, CancellationToken cancellationToken = default)
        {
            return await dbContext.Links
                .FirstOrDefaultAsync(l => l.ShortCode == shortCode, cancellationToken);
        }

        public async Task UpdateAsync(Link link, CancellationToken cancellationToken)
        {
            dbContext.Links.Update(link);
            try { await dbContext.SaveChangesAsync(cancellationToken); }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg)
            {
                throw new PersistenceExceptions(pg.SqlState switch
                {
                    PostgresErrorCodes.UniqueViolation => PersistenceErrorCodes.UniqueViolation, // 23505 → 409
                    PostgresErrorCodes.ForeignKeyViolation => PersistenceErrorCodes.ForeignKeyViolation, // 23503 → 409
                    PostgresErrorCodes.NotNullViolation or PostgresErrorCodes.InvalidTextRepresentation
                                                                 => PersistenceErrorCodes.InvalidData,  // 23502/22P02 → 400
                    _ => PersistenceErrorCodes.PersistenceFailed // → 500
                }, "Error al persistir la operacion.", ex);
            }
        }
    }
}
