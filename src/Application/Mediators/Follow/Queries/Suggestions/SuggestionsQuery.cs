using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Follow.Queries.Suggestions
{
    public class SuggestionsQuery : IRequest<IEnumerable<UserShortVm>>
    {
    }
}
