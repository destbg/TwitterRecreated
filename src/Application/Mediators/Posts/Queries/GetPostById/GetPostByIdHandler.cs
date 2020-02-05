using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Posts.Queries.GetPostById
{
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostVm>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;

        public GetPostByIdHandler(IPostRepository post, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<PostVm> Handle(GetPostByIdQuery request, CancellationToken cancellationToken) =>
            await _post.FindById(request.Id, _currentUser.User?.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(request.Id), request.Id);
    }
}
