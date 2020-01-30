using Application.Common.ViewModels;
using MediatR;

namespace Application.Messages.Command.MessagesRead
{
    public class MessagesReadCommand : IRequest<UserShortVm>
    {
        public long ChatId { get; set; }
    }
}
