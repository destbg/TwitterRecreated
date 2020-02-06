using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Tags.Command.CheckForTags
{
    public class CheckForTagsHandler : IRequestHandler<CheckForTagsCommand>
    {
        private readonly IHashTagRepository _hashTag;
        private readonly ICurrentUserService _currentUser;
        private readonly ICountryService _country;

        private static readonly Regex regex = new Regex("^[a-zA-Z0-9]+$");

        public CheckForTagsHandler(IHashTagRepository hashTag, ICurrentUserService currentUser, ICountryService country)
        {
            _hashTag = hashTag ?? throw new ArgumentNullException(nameof(hashTag));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _country = country ?? throw new ArgumentNullException(nameof(country));
        }

        public async Task<Unit> Handle(CheckForTagsCommand request, CancellationToken cancellationToken)
        {
            var (start, end) = request.Content.IndexOfNonRepeat('#', regex);
            if (start == -1)
                return Unit.Value;

            var tagString = request.Content[(start + 1)..end];

            var tag = await _hashTag.GetTag(tagString, cancellationToken);

            if (tag != null)
            {
                tag.Posts++;
                await _hashTag.Update(tag, cancellationToken);
            }
            else
            {
                await _hashTag.Create(new HashTag
                {
                    Tag = tagString,
                    Posts = 1,
                    Country = await _country.GetCountry()
                }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
