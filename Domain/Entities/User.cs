using Domain.Exceptions;
using System.Net.Mail;

namespace Domain.Entities
{
    public class User
    {

        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public User (string email, string passwordHash)
        {
            Validate(email, passwordHash);
            Email = email.ToLowerInvariant();
            PasswordHash = passwordHash;
            CreatedDate = DateTime.UtcNow;
        }

        private static void Validate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException("El email no debe estar vacio");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException("La contraseña debe ser obligatoria");
            }
            if (!MailAddress.TryCreate(email, out _))
            {
                throw new DomainException("El correo electronico no tiene el formato valido");
            }
        }

    }
}
