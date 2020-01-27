using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Tags.Queries.TopTags
{
    public class TopTagsQuery : IRequest<IEnumerable<HashTagVm>>
    {
    }
}
