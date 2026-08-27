using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands.Login
{
    public class LoginCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher) : IRequestHandler<LoginCommand, AuthRespondeDTO>
    {
        public async Task<AuthRespondeDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user == null || !passwordHasher.VerifyPassword(user, request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Credenciales invalidas");
            }

            var accessToken = tokenGenerator.GenerateAccessToken(user);
            var refreshToken = tokenGenerator.GenerateRefreshToken();
            user.AddRefreshToken(refreshToken);
            await userRepository.UpdateAsync(user, cancellationToken);

            return new AuthRespondeDTO(accessToken, refreshToken);

        }
    }
}
