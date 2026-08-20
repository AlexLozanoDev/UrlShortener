using Application.Common.Interfaces;
using Application.Links.Common.Exceptions;
using Application.Links.Events;
using Domain.ValueObjects;
using MediatR;

namespace Application.Links.Queries.GetLinkByShortCode
{
    public class GetLinkByShortCodeQueryHandler (ILinkRepository linkRepository, IPublisher publisher) : IRequestHandler<GetLinkByShortCodeQuery, string>
    {

        public async Task<string> Handle(GetLinkByShortCodeQuery query, CancellationToken cancellationToken)
        {
            var shortCode = new ShortCode(query.ShortCode);
            var link = await linkRepository.GetByShortCodeAsync(shortCode, cancellationToken);

            if (link == null)
            {
                throw new NotFoundException("El enlace solicitado no existe.");
            }

            await publisher.Publish(new LinkVisitedEvent(link.Id, DateTime.UtcNow, query.IpAddress, query.UserAgent), cancellationToken);

            return link.TargetUrl;
        }

    }
}
