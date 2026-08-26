using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken);
    }
}
