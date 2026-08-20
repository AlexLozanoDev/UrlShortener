using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Links.Queries.GetLinkByShortCode
{
    public record GetLinkByShortCodeQuery(string ShortCode, string? IpAddress, string? UserAgent) : IRequest<string>;
}
