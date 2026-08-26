using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Auth.Commands.Register
{
    public class RegisterCommandHandler (
        IUserRepository userRepository,
        IPasswordHasher passwordHasher) : IRequestHandler<RegisterCommand, Guid>
    {
        public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var userRecupered = await userRepository.GetUserAsync(request.email, cancellationToken);

            if (userRecupered != null)
            {
                throw new DomainException("El correo electrónico ya se encuentra en uso.");
            }

            var dummyUser = new User(request.email, request.password);
            var passwordHashed = passwordHasher.HashPassword(dummyUser, request.password);

            var user = new User(request.email, passwordHashed);

            userRepository.AddAsync(user, cancellationToken);

            return user.Id;
        }
    }
}
