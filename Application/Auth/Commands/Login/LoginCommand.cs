using MediatR;

namespace Application.Auth.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<AuthRespondeDTO>;

    public record AuthRespondeDTO(
        string AccessToken,
        string RefreshToken);
}
