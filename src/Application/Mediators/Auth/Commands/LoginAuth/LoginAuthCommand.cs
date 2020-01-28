using Application.Common.ViewModels;
using MediatR;

namespace Application.Auth.Commands.LoginAuth
{
    public class LoginAuthCommand : IRequest<AuthVm>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Recaptcha { get; set; }
    }
}
