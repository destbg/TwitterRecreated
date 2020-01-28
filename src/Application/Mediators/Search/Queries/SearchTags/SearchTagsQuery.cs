using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchTags
{
    public class SearchTagsQuery : IRequest<IEnumerable<HashTagVm>>
    {
        public string Search { get; set; }
    }
}
