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
        private readonly ILikedPostRepository _likedPost;

        public GetPostByIdHandler(IPostRepository post, ICurrentUserService currentUser, ILikedPostRepository likedPost)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
        }

        public async Task<PostVm> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _post.FindById(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(request.Id), request.Id);

            if (_currentUser.IsAuthenticated)
                post.IsLiked = await _likedPost.HasUserLikedPost(post.Id, _currentUser.User.Id, cancellationToken);

            return post;
        }
    }
}
