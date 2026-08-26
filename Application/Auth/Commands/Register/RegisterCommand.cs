using MediatR;

namespace Application.Auth.Commands.Register
{
    public record RegisterCommand(
        string email,
        string password
        ) : IRequest<Guid>;
}
