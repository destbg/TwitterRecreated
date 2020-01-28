using Application.Common.ViewModels;
using MediatR;

namespace Application.Posts.Queries.GetPostById
{
    public class GetPostByIdQuery : IRequest<PostVm>
    {
        public long Id { get; set; }
    }
}
