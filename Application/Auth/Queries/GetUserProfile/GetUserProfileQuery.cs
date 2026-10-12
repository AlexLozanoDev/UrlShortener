using MediatR;

namespace Application.Auth.Queries.GetUserProfile
{
    public record UserProfileResponse(
        Guid id,
        string Email
        );

    public record GetUserProfileQuery(Guid userId) : IRequest<UserProfileResponse>;
}
