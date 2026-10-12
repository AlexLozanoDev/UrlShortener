using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken);

        Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
