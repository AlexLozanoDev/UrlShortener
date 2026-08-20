using MediatR;

namespace Application.Links.Events
{
    public record LinkVisitedEvent(
        long LinkId, DateTime VisitedAt, string? IpAddress, string? UserAgent
        ) : INotification;
}
