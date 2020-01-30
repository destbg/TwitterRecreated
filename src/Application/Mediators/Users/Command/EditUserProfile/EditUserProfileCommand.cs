using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Command.EditUserProfile
{
    public class EditUserProfileCommand : IRequest<EditUserProfileResponse>
    {
        public IFormFile Thumbnail { get; set; }
        public IFormFile Image { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
