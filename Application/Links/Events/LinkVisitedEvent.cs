using Domain.ValueObjects;
using MediatR;

namespace Application.Links.Events
{
    public record LinkVisitedEvent(
        ShortCode ShortCode, DateTime VisitedAt, string? IpAddress, string? UserAgent
        ) : INotification;
}
