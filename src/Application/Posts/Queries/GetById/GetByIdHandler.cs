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

namespace Application.Posts.Queries.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, PostVm>
    {
        private readonly IRepository<Post> _post;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetByIdHandler(IRepository<Post> post, IMapper mapper, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<PostVm> Handle(GetByIdQuery request, CancellationToken cancellationToken) =>
            await _post.GetAll()
                .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(f => f.Id == request.Id)
                ?? throw new NotFoundException(nameof(request.Id), request.Id);
    }
}
