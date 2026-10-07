using Application.Common.Errors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Links.Events;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Links.Queries.GetLinkByShortCode
{
    public class GetLinkByShortCodeQueryHandler (
        ILinkRepository linkRepository, 
        ICacheService cacheService,
        IPublisher publisher,
        ILogger<GetLinkByShortCodeQueryHandler> logger) : IRequestHandler<GetLinkByShortCodeQuery, string>
    {

        public async Task<string> Handle(GetLinkByShortCodeQuery query, CancellationToken cancellationToken)
        {

            string cacheKey = $"link-{query.ShortCode}";

            var targetUrl = await cacheService.GetAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(targetUrl))
            {
                logger.LogInformation("Enlace {ShortCode} recuperado desde Redis.", query.ShortCode);
            }
            else
            {
                logger.LogInformation("Consultando enlace {ShortCode} desde PostgreSQL...", query.ShortCode);

                var shortCode = new ShortCode(query.ShortCode);
                var link = await linkRepository.GetByShortCodeAsync(shortCode, cancellationToken);
                
                if (link == null)
                {
                    throw new NotFoundException("El enlace solicitado no existe.", AppErrorCodes.LinkNotFound);
                }

                targetUrl = link.TargetUrl;

                await cacheService.SetAsync(cacheKey, targetUrl, TimeSpan.FromDays(7), cancellationToken);
                logger.LogInformation("Enlace {ShortCode} almacenado en Redis", query.ShortCode);
            }

            try
            {
                await publisher.Publish(new LinkVisitedEvent(new ShortCode(query.ShortCode), DateTime.UtcNow, query.IpAddress, query.UserAgent), cancellationToken);
            }
            catch (Exception)
            {
                logger.LogWarning("Error al publicar el evento de visita del enlace.");
            }

            return targetUrl;
        }

    }
}
