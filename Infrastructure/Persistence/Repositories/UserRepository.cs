using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository(ApplicationDbContext applicationDb) : IUserRepository
    {
        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await applicationDb.AddAsync(user, cancellationToken);
            await applicationDb.SaveChangesAsync(cancellationToken);
        }

        public async Task<User> GetUserAsync(string email, CancellationToken cancellationToken = default)
        {
            return await applicationDb.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
