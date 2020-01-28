using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchTags
{
    public class SearchTagsHandler : IRequestHandler<SearchTagsQuery, IEnumerable<HashTagVm>>
    {
        private readonly IHashTagRepository _hashTag;

        public SearchTagsHandler(IHashTagRepository hashTag)
        {
            _hashTag = hashTag ?? throw new ArgumentNullException(nameof(hashTag));
        }

        public async Task<IEnumerable<HashTagVm>> Handle(SearchTagsQuery request, CancellationToken cancellationToken) =>
            await _hashTag.SearchTags(request.Search, cancellationToken);
    }
}
