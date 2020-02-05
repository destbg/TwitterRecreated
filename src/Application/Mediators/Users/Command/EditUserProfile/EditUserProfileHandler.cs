using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Users.Command.EditUserProfile
{
    public class EditUserProfileHandler : IRequestHandler<EditUserProfileCommand, EditUserProfileResponse>
    {
        private readonly IUserManager _userManager;
        private readonly IImageService _image;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public EditUserProfileHandler(IUserManager userManager, IImageService image, IMapper mapper, ICurrentUserService currentUser)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _image = image ?? throw new ArgumentNullException(nameof(image));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<EditUserProfileResponse> Handle(EditUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = _currentUser.User;

            if (request.Image?.ContentType.StartsWith("image") == true)
                user.Image = await _image.SaveImage(request.Image, "jpg");

            if (request.Thumbnail?.ContentType.StartsWith("image") == true)
                user.Thumbnail = await _image.SaveImage(request.Thumbnail, "jpg");

            if (request.Description != null)
                user.Description = request.Description;

            if (request.DisplayName != null)
                user.DisplayName = request.DisplayName;

            await _userManager.UpdateUser(user);
            return _mapper.Map<EditUserProfileResponse>(user);
        }
    }
}
