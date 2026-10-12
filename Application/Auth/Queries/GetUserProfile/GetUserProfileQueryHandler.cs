using Application.Common.Errors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetUserProfileQuery, UserProfileResponse>
    {

        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("El usuario autenticado no existe en el sistema", AppErrorCodes.AuthenticationRequired);
            }

            return new UserProfileResponse(user.Id, user.Email);
        }
    }
}
