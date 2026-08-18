using Domain.Exceptions;

namespace Domain.Entities
{
    public class LinkClickStats
    {
        public long Id { get; private set; }
        public long LinkId {  get; private set; }
        public DateTime ClickedAt { get; private set; }
        public string? IpAddress { get; private set; }
        public string? UserAgent { get; private set; }


        public LinkClickStats(long linkId, string? ipAddress, string? userAgent)
        {

            Validate(linkId);

            LinkId = linkId;
            ClickedAt = DateTime.UtcNow;
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }

        private static void Validate(long linkId)
        {
            if (linkId <= 0)
            {
                throw new DomainException("El id del link relacionado es invalido");
            }
        }



    }
}
