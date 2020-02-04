using Domain.Entities;
using MediatR;

namespace Application.Chats.Command.CallRequest
{
    public class CallRequestCommand : IRequest<AppUser>
    {
        public long Id { get; set; }
        public string Data { get; set; }
    }
}
