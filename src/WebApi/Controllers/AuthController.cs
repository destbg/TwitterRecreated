using Application.Auth.Commands.CurrentUser;
using Application.Auth.Commands.LoginAuth;
using Application.Auth.Commands.RegisterAuth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AuthController : BaseController
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginAuthCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterAuthCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> CurrentUser(CurrentUserCommand command) =>
            Ok(await Mediator.Send(command));
    }
}
