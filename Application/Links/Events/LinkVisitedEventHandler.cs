using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Links.Events
{
    public class LinkVisitedEventHandler (
        IStatsRepository statsRepository,
        ILogger<LinkVisitedEventHandler> logger
        ) : INotificationHandler<LinkVisitedEvent>
    {
        public async Task Handle(LinkVisitedEvent notification, CancellationToken cancellationToken)
        {

            var linkStats = new LinkClickStats(notification.LinkId, notification.IpAddress, notification.UserAgent);

            await statsRepository.AddAsync(linkStats, cancellationToken);

            logger.LogInformation($"[Estadísticas] Enlace {notification.LinkId} visitado a las {notification.VisitedAt}");
        }
    }
}
