using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Repositories;
using Application.Common.Validators;
using Application.Common.ViewModels;
using Application.Notifications.Command.CreateNotification;
using Application.Tags.Command.CheckForTags;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Replies.Command.CreatePostReply
{
    public class CreatePostReplyHandler : IRequestHandler<CreatePostReplyCommand>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly IImageService _imageService;
        private readonly IVideoService _videoService;
        private readonly IMapper _mapper;
        private readonly IMainHubService _mainHub;
        private readonly IMediator _mediator;

        public CreatePostReplyHandler(IPostRepository post, ICurrentUserService currentUser, IImageService imageService, IVideoService videoService, IMapper mapper, IMainHubService mainHub, IMediator mediator)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(CreatePostReplyCommand request, CancellationToken cancellationToken)
        {
            var postToReply = await _post.GetById(request.ReplyId, cancellationToken)
                ?? throw new NotFoundException("Reply Id", request.ReplyId);

            postToReply.Comments++;
            await _post.Update(postToReply, cancellationToken);

            string videoLink = null;
            PostType validFiles = default;
            if (request.Files == null)
                videoLink = HandlerValidators.VideoLink(request.Content);
            else validFiles = HandlerValidators.GetFileTypes(request.Files);

            var post = new Post
            {
                Content = request.Content,
                PollEnd = request.PollEnd,
                Video = videoLink,
                UserId = _currentUser.User.Id,
                ReplyId = postToReply.Id
            };

            await _post.Create(post, cancellationToken);

            if (request.Gif != null)
            {
                post.Images.Add(new PostImage
                {
                    Image = request.Gif.ToString(),
                    PostId = post.Id
                });
                await _post.Update(post, cancellationToken);
                return await ExecuteAndReturn(post, postToReply);
            }

            if (request.Poll != null && request.PollEnd.HasValue || videoLink != null || request.Files == null)
                return await ExecuteAndReturn(post, postToReply);

            switch (validFiles)
            {
                case PostType.Images:
                    var images = await _imageService.SaveImages(request.Files);
                    foreach (var image in images)
                    {
                        post.Images.Add(new PostImage
                        {
                            Image = image,
                            PostId = post.Id
                        });
                    }
                    break;
                case PostType.Video:
                    post.Video = await _videoService.CreateVideo(request.Files[0]);
                    break;
                case PostType.Gif:
                    var gif = await _imageService.SaveImage(request.Files[0], ".gif");
                    post.Images.Add(new PostImage
                    {
                        Image = gif,
                        PostId = post.Id
                    });
                    break;
            }

            await _post.Update(post, cancellationToken);

            return await ExecuteAndReturn(post, postToReply);
        }

        private async Task<Unit> ExecuteAndReturn(Post post, Post postToReply)
        {
            await _mediator.Send(new CheckForTagsCommand { Content = post.Content });

            if (_currentUser.User.Verified)
            {
                await _mediator.Send(new CreateNotificationCommand
                {
                    NotificationType = NotificationType.Reply,
                    UserId = post.UserId,
                    PostId = post.Id
                });
            }

            post.User = _currentUser.User;
            post.Reply = postToReply;

            await _mainHub.SendPost(_mapper.Map<PostVm>(post));
            return Unit.Value;
        }
    }
}
