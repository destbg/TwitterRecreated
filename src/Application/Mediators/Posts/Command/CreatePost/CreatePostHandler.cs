using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Repositories;
using Application.Common.Validators;
using Application.Common.ViewModels;
using Application.Notifications.Command.CreateMultipleNotifications;
using Application.Tags.Command.CheckForTags;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Posts.Command.CreatePost
{
    public class CreatePostHandler : IRequestHandler<CreatePostCommand>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly IImageService _imageService;
        private readonly IVideoService _videoService;
        private readonly IMapper _mapper;
        private readonly IMainHubService _mainHub;
        private readonly IMediator _mediator;

        public CreatePostHandler(IPostRepository post, ICurrentUserService currentUser, IImageService imageService, IVideoService videoService, IMapper mapper, IMainHubService mainHub, IMediator mediator)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
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
                UserId = _currentUser.User.Id
            };

            await _post.Create(post, cancellationToken);

            if (request.Gif != null)
            {
                post.Images.Add(new PostImage
                {
                    Image = request.Gif,
                    PostId = post.Id
                });
                await _post.Update(post, cancellationToken);
                return await ExecuteAndReturn(post);
            }

            if (request.Poll != null && request.PollEnd.HasValue)
            {
                foreach (var option in request.Poll)
                {
                    post.Poll.Add(new PollOption
                    {
                        Option = option,
                        PostId = post.Id
                    });
                }
                await _post.Update(post, cancellationToken);
                return await ExecuteAndReturn(post);
            }

            if (videoLink != null || request.Files == null || request.Files.Count == 0)
                return await ExecuteAndReturn(post);

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

            return await ExecuteAndReturn(post);
        }

        private async Task<Unit> ExecuteAndReturn(Post post)
        {
            await _mediator.Send(new CheckForTagsCommand { Content = post.Content });

            if (_currentUser.User.Verified)
            {
                await _mediator.Send(new CreateMultipleNotificationsCommand
                {
                    NotificationType = NotificationType.Post,
                    PostId = post.Id
                });
            }

            post.User = _currentUser.User;
            await _mainHub.SendPost(_mapper.Map<PostVm>(post));
            return Unit.Value;
        }
    }
}
