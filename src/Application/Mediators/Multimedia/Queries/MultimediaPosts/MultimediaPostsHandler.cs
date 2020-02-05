using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Multimedia.Queries.MultimediaPosts
{
    public class MultimediaPostsHandler : IRequestHandler<MultimediaPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;

        public MultimediaPostsHandler(IPostRepository post, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(MultimediaPostsQuery request, CancellationToken cancellationToken) =>
            await _post.MultimediaPosts(request.Username, _currentUser.User.Id, request.Skip, cancellationToken);
    }
}
