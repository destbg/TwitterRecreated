using Application.Common.Interfaces;
using Application.Common.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Posts.Queries.FollowersPosts
{
    public class FollowersPostsHandler : IRequestHandler<FollowersPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IRepository<Post> _post;
        private readonly IMapper _mapper;

        public FollowersPostsHandler(IRepository<Post> post, IMapper mapper)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PostVm>> Handle(FollowersPostsQuery request, CancellationToken cancellationToken) =>
            await _post.GetAll()
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .Where(f => f.PostedOn > request.Skip)
                .OrderByDescending(f => f.PostedOn)
                .Take(50)
                .ToListAsync();
    }
}
