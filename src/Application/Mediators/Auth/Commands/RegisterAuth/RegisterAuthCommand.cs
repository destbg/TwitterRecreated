using Application.Common.ViewModels;
using MediatR;

namespace Application.Auth.Commands.RegisterAuth
{
    public class RegisterAuthCommand : IRequest<AuthVm>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Recaptcha { get; set; }
    }
}
