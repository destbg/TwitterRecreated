﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Reposts.Command.CreateRepost
{
    public class CreateRepostHandler : IRequestHandler<CreateRepostCommand>
    {
        private readonly IRepostRepository _repost;
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;

        public CreateRepostHandler(IRepostRepository repost, IPostRepository post, ICurrentUserService currentUser)
        {
            _repost = repost ?? throw new ArgumentNullException(nameof(repost));
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(CreateRepostCommand request, CancellationToken cancellationToken)
        {
            var post = await _post.GetById(request.Id, cancellationToken)
                ?? throw new NotFoundException("Post Id", request.Id);

            if (post.UserId == _currentUser.UserId)
                throw new BadRequestException("You can't repost your own post");

            var repost = await _repost.FindByUserAndPost(_currentUser.UserId, request.Id, cancellationToken);

            if (repost == default)
            {
                await _repost.Create(new Repost
                {
                    Content = request.Content,
                    PostId = request.Id,
                    UserId = _currentUser.UserId
                }, cancellationToken);
                post.Reposts++;
                await _post.Update(post, cancellationToken);
            }
            else
            {
                await _repost.Delete(repost, cancellationToken);
                post.Reposts--;
                await _post.Update(post, cancellationToken);
            }

            return Unit.Value;
        }
    }
}