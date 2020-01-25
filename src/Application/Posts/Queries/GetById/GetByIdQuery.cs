using Application.Common.ViewModels;
using MediatR;

namespace Application.Posts.Queries.GetById
{
    public class GetByIdQuery : IRequest<PostVm>
    {
        public long Id { get; set; }
    }
}
