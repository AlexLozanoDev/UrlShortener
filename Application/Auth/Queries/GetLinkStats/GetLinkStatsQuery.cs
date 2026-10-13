using MediatR;

namespace Application.Auth.Queries.GetLinkStats
{
    public record ClicksStatItemDto(
        DateTime ClickedAt,
        string? IpAddress,
        string? UserAgent);

    public record LinkStatsResponse(
        string Code,
        string TargetUrl,
        int TotalClicksAllTime,
        IEnumerable<ClicksStatItemDto> Stats);

    public record GetLinksStatsQuery(string ShortCode, Guid OwnerId) : IRequest<LinkStatsResponse>;
}
