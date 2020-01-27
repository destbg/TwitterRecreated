using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tags.Queries.TopTags
{
    public class TopTagsHandler : IRequestHandler<TopTagsQuery, IEnumerable<HashTagVm>>
    {
        private readonly IHashTagRepository _hashTag;
        private readonly ICountryService _country;

        public TopTagsHandler(IHashTagRepository hashTag, ICountryService country)
        {
            _hashTag = hashTag ?? throw new ArgumentNullException(nameof(hashTag));
            _country = country ?? throw new ArgumentNullException(nameof(country));
        }

        public async Task<IEnumerable<HashTagVm>> Handle(TopTagsQuery request, CancellationToken cancellationToken) =>
            await _hashTag.GetTopTags(await _country.GetCountry(), cancellationToken);
    }
}
