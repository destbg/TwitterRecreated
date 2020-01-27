using Application.Common.Exceptions;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Posts.Queries.GetPostById
{
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostVm>
    {
        private readonly IPostRepository _post;

        public GetPostByIdHandler(IPostRepository post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<PostVm> Handle(GetPostByIdQuery request, CancellationToken cancellationToken) =>
            await _post.FindById(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(request.Id), request.Id);
    }
}
