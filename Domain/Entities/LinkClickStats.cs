using Domain.ValueObjects;

namespace Domain.Entities
{
    public class LinkClickStats
    {
        public long Id { get; private set; }
        public ShortCode ShortCode {  get; private set; }
        public DateTime ClickedAt { get; private set; }
        public string? IpAddress { get; private set; }
        public string? UserAgent { get; private set; }


        public LinkClickStats(ShortCode shortCode, string? ipAddress, string? userAgent)
        {
            ShortCode = shortCode;
            ClickedAt = DateTime.UtcNow;
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }



    }
}
