using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Repositories;
using Application.Common.Validators;
using Application.Common.ViewModels;
using AutoMapper;
using Common;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Posts.Command.CreatePost
{
    public class CreatePostHandler : IRequestHandler<CreatePostCommand>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _dateTime;
        private readonly IImageService _imageService;
        private readonly IVideoService _videoService;
        private readonly IMapper _mapper;
        private readonly IMainHubService _mainHub;

        public CreatePostHandler(IPostRepository post, ICurrentUserService currentUser, IDateTime dateTime, IImageService imageService, IVideoService videoService, IMapper mapper, IMainHubService mainHub)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
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
                UserId = _currentUser.UserId,
                PollEnd = request.PollEnd,
                PostedOn = _dateTime.Now,
                Video = videoLink
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
                return await SendAndReturn(post);
            }

            if (request.Poll != null && request.PollEnd.HasValue || videoLink != null || request.Files == null)
                return await SendAndReturn(post);

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

            return await SendAndReturn(post);
        }

        private async Task<Unit> SendAndReturn(Post post)
        {
            await _mainHub.SendPost(_mapper.Map<PostVm>(post));
            return Unit.Value;
        }
    }
}
