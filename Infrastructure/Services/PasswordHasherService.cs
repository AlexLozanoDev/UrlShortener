using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasher
    {

        private readonly PasswordHasher<User> _hashProvider = new();

        public string HashPassword(User user, string password)
        {
            return _hashProvider.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string providedPassword, string passwordHash)
        {
            var result = _hashProvider.VerifyHashedPassword(user, passwordHash, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
