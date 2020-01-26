using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Posts.Queries.GetPostById
{
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostVm>
    {
        private readonly IRepository<Post> _post;
        private readonly IMapper _mapper;

        public GetPostByIdHandler(IRepository<Post> post, IMapper mapper)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PostVm> Handle(GetPostByIdQuery request, CancellationToken cancellationToken) =>
            await _post.GetAll()
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(f => f.Id == request.Id)
                ?? throw new NotFoundException(nameof(request.Id), request.Id);
    }
}
