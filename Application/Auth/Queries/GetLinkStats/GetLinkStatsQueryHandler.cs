using Application.Common.Errors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.Auth.Queries.GetLinkStats
{
    public class GetLinkStatsQueryHandler(
        ILinkRepository linkRepository,
        IStatsRepository statsRepository)
        : IRequestHandler<GetLinksStatsQuery, LinkStatsResponse>
    {
        private readonly ILinkRepository _linkRepository = linkRepository;
        private readonly IStatsRepository _statsRepository = statsRepository;
        public async Task<LinkStatsResponse> Handle(GetLinksStatsQuery request, CancellationToken cancellationToken)
        {
            var shortCodeObj = new ShortCode(request.ShortCode);
            var link = await _linkRepository.GetByShortCodeAsync(shortCodeObj, cancellationToken);

            if (link == null)
            {
                throw new NotFoundException("El enlace solicitado no existe", AppErrorCodes.LinkNotFound);
            }

            if (link.OwnerId != request.OwnerId)
            {
                throw new ForbiddenException("No tienes permisos para ver las estadisticas de este enlace");
            }

            var stats = await _statsRepository.GetByLinkIdAsync(link.ShortCode, cancellationToken);

            var statDtos = stats.Select(s => new ClicksStatItemDto(
                ClickedAt: s.ClickedAt,
                IpAddress: s.IpAddress,
                UserAgent: s.UserAgent
                )); 

            int totalClicksAllTime = stats.Count();

            return new LinkStatsResponse(
                Code: link.ShortCode.Value,
                TargetUrl: link.TargetUrl,
                TotalClicksAllTime: totalClicksAllTime,
                Stats: statDtos
            );
        }
    }
}
