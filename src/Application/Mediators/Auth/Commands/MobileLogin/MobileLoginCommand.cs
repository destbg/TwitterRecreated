using Application.Common.ViewModels;
using MediatR;

namespace Application.Auth.Commands.MobileLogin
{
    public class MobileLoginCommand : IRequest<AuthVm>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
