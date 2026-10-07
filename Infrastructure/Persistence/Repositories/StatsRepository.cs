using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Persistence.Repositories
{
    public class StatsRepository(ApplicationDbContext context) : IStatsRepository
    {
        public async Task AddAsync(LinkClickStats stats, CancellationToken cancellationToken = default)
        {
            await context.LinkClickStats.AddAsync(stats, cancellationToken);
            try { await context.SaveChangesAsync(cancellationToken); }
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
