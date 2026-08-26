using Domain.ValueObjects;
using MediatR;

namespace Application.Links.Queries.GetLinkByShortCode
{
    public record GetLinkByShortCodeQuery(string ShortCode, string? IpAddress, string? UserAgent) : IRequest<string>;
}
