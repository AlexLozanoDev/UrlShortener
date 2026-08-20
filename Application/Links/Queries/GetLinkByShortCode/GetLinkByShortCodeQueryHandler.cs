using Application.Common.Interfaces;
using Application.Links.Common.Exceptions;
using Domain.ValueObjects;

namespace Application.Links.Queries.GetLinkByShortCode
{
    public class GetLinkByShortCodeQueryHandler (ILinkRepository linkRepository)
    {

        public async Task<string> Handle(GetLinkByShortCodeQuery query)
        {
            var shortCode = new ShortCode(query.code);
            var link = await linkRepository.GetByShortCodeAsync(shortCode);
            return link == null ? throw new NotFoundException("Link no encontrado") : link.TargetUrl;
        }

    }
}
