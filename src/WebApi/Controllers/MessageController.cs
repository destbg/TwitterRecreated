using System;
using System.Threading.Tasks;
using Application.Messages.Command.CreateMessage;
using Application.Messages.Queries.Messages;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {
        private readonly IDateTime _date;

        public MessageController(IDateTime date)
        {
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [HttpGet("{id}/{skip?}")]
        public async Task<IActionResult> Messages(long id, DateTime? skip) =>
            Ok(await Mediator.Send(new MessagesQuery
            {
                ChatId = id,
                Skip = skip ?? _date.MinDate
            }));

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
