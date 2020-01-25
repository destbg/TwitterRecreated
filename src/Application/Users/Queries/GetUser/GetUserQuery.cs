using Application.Common.ViewModels;
using MediatR;

namespace Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserVm>
    {
        public string Username { get; set; }
    }
}
