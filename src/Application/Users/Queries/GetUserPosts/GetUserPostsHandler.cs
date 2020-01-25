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

namespace Application.Users.Queries.GetUserPosts
{
    public class GetUserPostsHandler : IRequestHandler<GetUserPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IRepository<Post> _posts;
        private readonly IMapper _mapper;

        public GetUserPostsHandler(IRepository<Post> posts, IMapper mapper)
        {
            _posts = posts ?? throw new ArgumentNullException(nameof(posts));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PostVm>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken) =>
            await _posts.GetAll()
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .Where(f => f.PostedOn > request.Skip)
                .Take(50)
                .ToListAsync(cancellationToken);
    }
}
