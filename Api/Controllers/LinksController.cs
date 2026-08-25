using Api.DTOs;
using Application.Links.Commands.CreateLink;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("v1/links")]
    public class LinksController(IMediator mediator) : ControllerBase
    {

        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateLink([FromBody] CreateLinkRequest request, CancellationToken cancellationToken)
        {

            var command = new CreateLinkCommand(request.TargetURL, request.Alias, request.UserId);

            var result = await _mediator.Send(command, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, result);

        }
    }
}
