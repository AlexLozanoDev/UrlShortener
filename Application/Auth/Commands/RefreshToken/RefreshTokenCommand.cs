using Application.Auth.Commands.Login;
using MediatR;

namespace Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(
        string AccessToken,
        string RefreshToken) : IRequest<AuthRespondeDTO>;
}
