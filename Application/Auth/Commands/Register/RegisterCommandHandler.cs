using Application.Common.Errors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Auth.Commands.Register
{
    public class RegisterCommandHandler (
        IUserRepository userRepository,
        IPasswordHasher passwordHasher) : IRequestHandler<RegisterCommand, Guid>
    {
        public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var userRecupered = await userRepository.GetUserByEmailAsync(request.email, cancellationToken);

            if (userRecupered != null)
            {
                throw new ConflictException("El correo electrónico ya se encuentra en uso.", AppErrorCodes.EmailAlreadyRegistered);
            }

            var dummyUser = new User(request.email, request.password);
            var passwordHashed = passwordHasher.HashPassword(dummyUser, request.password);

            var user = new User(request.email, passwordHashed);

            await userRepository.AddAsync(user, cancellationToken);

            return user.Id;
        }
    }
}
