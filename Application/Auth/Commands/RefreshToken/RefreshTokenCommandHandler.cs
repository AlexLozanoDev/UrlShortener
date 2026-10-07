using Application.Auth.Commands.Login;
using Application.Common.Errors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator tokenGenerator) : IRequestHandler<RefreshTokenCommand, AuthRespondeDTO>
    {
        public async Task<AuthRespondeDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByRefreshTokenAsync(request.RefreshToken, cancellationToken);

            var existingToken = user.RefreshTokens
                .FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (existingToken is null || !existingToken.IsActive)
            {
                throw new AuthenticationFailedException("El token es inválido o ha caducado.", AppErrorCodes.InvalidRefreshToken);
            }

            existingToken.Revoke();

            var newAccessToken = tokenGenerator.GenerateAccessToken(user);
            var newRefreshToken = tokenGenerator.GenerateRefreshToken();

            user.AddRefreshToken(newRefreshToken);

            await userRepository.UpdateAsync(user, cancellationToken);

            return new AuthRespondeDTO(newAccessToken, newRefreshToken);
        }
    }
}
