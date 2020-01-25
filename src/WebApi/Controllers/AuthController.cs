using Application.Auth.Commands.LoginAuth;
using Application.Auth.Commands.RegisterAuth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AuthController : BaseController
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginAuthCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterAuthCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
