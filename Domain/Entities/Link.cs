using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Link
    {
        public long Id { get; private set; }
        public ShortCode ShortCode { get; private set; }
        public string TargetUrl { get; private set; }
        public bool IsCustomAlias { get; private set; }
        public Guid? OwnerId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? ExpirationDate { get; private set; }


        public Link(ShortCode shortCode, string targetUrl, bool isCustomAlias, Guid? ownerId)
        {
            Validate(targetUrl, isCustomAlias, ownerId);
            
            ExpirationDate = ownerId == null ? DateTime.UtcNow.AddDays(30) : null;
            ShortCode = shortCode;
            TargetUrl = targetUrl;
            IsCustomAlias = isCustomAlias;
            OwnerId = ownerId;
            CreatedDate = DateTime.UtcNow;

        }

        private static void Validate(string targetUrl, bool isCustomAlias, Guid? ownerId)
        {
            if (string.IsNullOrWhiteSpace(targetUrl))
            {
                throw new DomainException("La url no puede ir vacia o nula");
            }
            if (!Uri.TryCreate(targetUrl, UriKind.Absolute, out _))
            {
                throw new DomainException("La URL no tiene un formato valido");
            }
            if (isCustomAlias && ownerId == null)
            {
                throw new DomainException("Un alias personalizado debe tener un usuario propietario");
            }
        }
    }
}
