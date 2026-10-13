using Api.Common.Responses;
using Api.DTOs;
using Api.Extensions;
using Application.Auth.Queries.GetLinkStats;
using Application.Auth.Queries.GetMyLinks;
using Application.Common.Errors;
using Application.Common.Exceptions;
using Application.Links.Commands.CreateLink;
using Application.Links.Queries.GetLinkByShortCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/trimit")]
    public class LinksController(IMediator mediator) : ControllerBase
    {

        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<LinkCreatedResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<LinkCreatedResponse>>> CreateLink(
            [FromBody] CreateLinkRequest request, 
            CancellationToken cancellationToken)
        {
            Guid? userId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(claimValue, out var parsedId))
                {
                    userId = parsedId;
                }
            }

            var command = new CreateLinkCommand(request.TargetURL, request.Alias, userId);

            var result = await _mediator.Send(command, cancellationToken);

            return this.ApiCreated(
                new LinkCreatedResponse(result),
                "Enlace creado correctamente");

        }

        [HttpGet("/{code}")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RedirectToTarget(
            string code, 
            IMediator mediator, 
            CancellationToken cancellationToken)
        {

            string? userAgent = Request.Headers["User-Agent"].ToString();
            string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var query = new GetLinkByShortCodeQuery(code, ipAddress, userAgent);
            var tagetUrl = await mediator.Send(query, cancellationToken);

            if (string.IsNullOrEmpty(tagetUrl))
            {
                return NotFound();
            }

            return Redirect(tagetUrl);
        }

        [HttpGet("mine")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<LinkItemDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<PagedResponse<LinkItemDTO>>>> GetMyLinks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(claimValue, out var userId))
            {
                throw new AuthenticationFailedException("Token inválido.", AppErrorCodes.AuthenticationRequired);
            }

            var query = new GetMyLinksQuery(userId, page, pageSize);
            var result = await _mediator.Send(query, cancellationToken);

            return this.ApiOk(result, "Enlaces obtenidos correctamente");
        }

        [HttpGet("{code}/stats")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LinkStatsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LinkStatsResponse>>> GetLinkStats(
            string code,
            CancellationToken cancellationToken)
        {
            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(claimValue, out var userId))
            {
                throw new AuthenticationFailedException("Token inválido.", AppErrorCodes.AuthenticationRequired);
            }

            var query = new GetLinksStatsQuery(code, userId);
            var result = await _mediator.Send(query, cancellationToken);

            return this.ApiOk(result, "Estadísticas obtenidas correctamente");
        }

    }
}
