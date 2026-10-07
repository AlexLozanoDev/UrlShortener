using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository(ApplicationDbContext applicationDb) : IUserRepository
    {
        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await applicationDb.AddAsync(user, cancellationToken);
            try { await applicationDb.SaveChangesAsync(cancellationToken); }
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

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await applicationDb.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await applicationDb.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken), cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            applicationDb.Users.Update(user);
            try { await applicationDb.SaveChangesAsync(cancellationToken); }
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
