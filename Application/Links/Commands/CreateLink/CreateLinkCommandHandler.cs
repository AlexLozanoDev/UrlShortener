using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Links.Commands.CreateLink
{
    public class CreateLinkCommandHandler (ILinkRepository linkRepository) : IRequestHandler<CreateLinkCommand, string>
    {
        public async Task<string> Handle(CreateLinkCommand command, CancellationToken cancellationToken)
        {

            ShortCode? shortCode = null;

            if (command.CustomAlias != null)
            {
                shortCode = new(command.CustomAlias);
                var existingUrl = await linkRepository.GetByShortCodeAsync(shortCode);
                if (existingUrl != null)
                {
                    throw new Exception("El alias ya está en uso");
                }
            }
            else
            {
                bool isAvailable = false;
                while (!isAvailable)
                {
                    shortCode = CreateShortCode();
                    var existing = await linkRepository.GetByShortCodeAsync(shortCode);
                    if (existing == null)
                    {
                        isAvailable = true;
                    }
                }
            }


            var newLink = new Link(shortCode, command.TargetUrl, command.CustomAlias != null, command.UserId);


            await linkRepository.AddAsync(newLink);

            
            return shortCode.Value;
        }

        private static ShortCode CreateShortCode(int length = 7)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                int randomIndex = Random.Shared.Next(alphabet.Length);
                chars[i] = alphabet[randomIndex];
            }

            return new ShortCode(new string(chars));
        }
    }

}
