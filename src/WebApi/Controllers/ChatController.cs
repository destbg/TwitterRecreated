using System.Threading.Tasks;
using Application.Chats.Command.ChangeColor;
using Application.Chats.Command.CreateChat;
using Application.Chats.Command.CreateGroupChat;
using Application.Chats.Queries.UserChats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class ChatController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> UserChats() =>
            Ok(await Mediator.Send(new UserChatsQuery()));

        [HttpPost]
        public async Task<IActionResult> CreateChat(CreateChatCommand command) =>
            Ok(await Mediator.Send(command));

        [HttpPost("Group")]
        public async Task<IActionResult> CreateGroupChat(CreateGroupChatCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("Color")]
        public async Task<IActionResult> ChangeColor(ChangeColorCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
