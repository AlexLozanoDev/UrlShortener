using MediatR;

namespace Application.Links.Commands.CreateLink
{
    public record CreateLinkCommand(
        string TargetUrl,
        string? CustomAlias,
        Guid? UserId
    ) : IRequest<string>;
}
