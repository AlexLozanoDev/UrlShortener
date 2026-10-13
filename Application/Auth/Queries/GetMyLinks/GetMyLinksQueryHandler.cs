using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Auth.Queries.GetMyLinks
{
    public class GetMyLinksQueryHandler(
        ILinkRepository linkRepository,
        IStatsRepository statsRepository)
        : IRequestHandler<GetMyLinksQuery, PagedResponse<LinkItemDTO>>
    {
        private readonly ILinkRepository _linkRepository = linkRepository;
        private readonly IStatsRepository _statsRepository = statsRepository;

        public async Task<PagedResponse<LinkItemDTO>> Handle(GetMyLinksQuery request, CancellationToken cancellationToken)
        {

            var (links, totalCount) = await _linkRepository.GetByOwnerIdPagedAsync(
                request.OwnerId,
                request.Page,
                request.PageSize,
                cancellationToken);

            var linkDtos = new List<LinkItemDTO>();

            foreach (var link in links)
            {
                var stats = await _statsRepository.GetByLinkIdAsync(link.ShortCode, cancellationToken);
                int totalClicks = stats.Count();

                linkDtos.Add(new LinkItemDTO(
                    code: link.ShortCode.Value,
                    TargetUrl: link.TargetUrl,
                    IsCustomAlias: link.IsCustomAlias,
                    TotalClicks: totalClicks,
                    CreatedAt: link.CreatedDate
                    ));
            }

            int totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<LinkItemDTO>(
                Items: linkDtos,
                Page: request.Page,
                PageSize: request.PageSize,
                TotalCount: totalCount,
                TotalPages: totalPages == 0 ? 1 : totalPages
                );
        }
    }
}
