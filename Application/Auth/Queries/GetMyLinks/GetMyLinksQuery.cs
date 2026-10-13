using MediatR;

namespace Application.Auth.Queries.GetMyLinks
{
    public record LinkItemDTO(
        string code,
        string TargetUrl,
        bool IsCustomAlias,
        int TotalClicks,
        DateTime CreatedAt);

    public record PagedResponse<T>(
        IEnumerable<T> Items,
        int Page,
        int PageSize,
        int TotalCount,
        int TotalPages);

    public record GetMyLinksQuery(Guid OwnerId, int Page = 1, int PageSize = 20) : IRequest<PagedResponse<LinkItemDTO>>;
}
