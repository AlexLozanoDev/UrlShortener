using Domain.Exceptions;

namespace Domain.Entities
{
    public class RefreshToken
    {

        public long Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime CreatedDate { get; private set; }


        public RefreshToken(Guid userId, string token)
        {

            Validate(userId, token);

            UserId = userId;
            Token = token;
            ExpiresAt = DateTime.UtcNow.AddHours(24);
            IsRevoked = false;
            CreatedDate = DateTime.UtcNow;

        }

        private static void Validate(Guid userId, string token)
        {
            if (userId == Guid.Empty)
            {
                throw new DomainException("El id de usuario no puede estar vacio");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new DomainException("El token debe de estar presente");
            }
        }

        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;

    }
}
