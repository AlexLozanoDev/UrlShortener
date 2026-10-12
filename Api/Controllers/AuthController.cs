using Api.Common.Responses;
using Api.DTOs;
using Api.Extensions;
using Application.Auth.Commands.Login;
using Application.Auth.Commands.RefreshToken;
using Application.Auth.Commands.Register;
using Application.Auth.Queries.GetUserProfile;
using Application.Common.Errors;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController (IMediator mediator) : ControllerBase
    {

        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<RegisterResponse>>> Register(
            [FromBody] RegisterCommand command, 
            CancellationToken cancellationToken)
        {
            var userId = await _mediator.Send(command, cancellationToken);
            return this.ApiCreated(new RegisterResponse(userId), "Usuario registrado correctamente");
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthRespondeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<AuthRespondeDTO>>> Login(
            [FromBody] LoginCommand command, 
            CancellationToken cancellationToken)
        {
            var tokens = await _mediator.Send(command, cancellationToken);
            return this.ApiOk(tokens, "Inicio de sesión exitoso");
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ApiResponse<AuthRespondeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<AuthRespondeDTO>>> RefreshToken(
            [FromBody] RefreshTokenCommand command, 
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return this.ApiOk(response, "Token renovado correctamente");
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserProfileResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<UserProfileResponse>>> GetCurrentUser(CancellationToken cancellationToken)
        {
            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(claimValue, out var userId))
            {
                throw new AuthenticationFailedException("Token de acceso invalido o corrupto", AppErrorCodes.AuthenticationRequired);
            }

            var query = new GetUserProfileQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);

            return this.ApiOk(result, "Informacion de sesion obtenida correctamente");
        }

    }
}
