using Application.Common.Errors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Utils;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Links.Commands.CreateLink
{
    public class CreateLinkCommandHandler (ILinkRepository linkRepository) : IRequestHandler<CreateLinkCommand, string>
    {
        public async Task<string> Handle(CreateLinkCommand command, CancellationToken cancellationToken)
        {

            if (!string.IsNullOrEmpty(command.CustomAlias) && command.UserId == null)
            {
                throw new AuthenticationFailedException("Se requiere autenticación para reclamar un alias personalizado.", AppErrorCodes.AuthenticationRequired);
            }

            ShortCode shortCode;

            if (!string.IsNullOrEmpty(command.CustomAlias))
            {
                shortCode = new ShortCode(command.CustomAlias);
                var existingUrl = await linkRepository.GetByShortCodeAsync(shortCode);

                if (existingUrl != null)
                {
                    throw new ConflictException("El alias personalizado ya está en uso.", AppErrorCodes.AliasAlreadyTaken);
                }

                var customLink = new Link(shortCode, command.TargetUrl, isCustomAlias: true, ownerId: command.UserId);
                await linkRepository.AddAsync(customLink, cancellationToken);

                return shortCode.Value;

            }
            else
            {
                var temCode = new ShortCode("temp");
                var newLink = new Link(temCode, command.TargetUrl, isCustomAlias: false, ownerId: null);

                await linkRepository.AddAsync(newLink, cancellationToken);

                var generatedCode = Base62Encoder.Encode(newLink.Id);

                newLink.UpdateCode(new ShortCode(generatedCode));

                await linkRepository.UpdateAsync(newLink, cancellationToken);

                return generatedCode;
            }

        }

    }

}
