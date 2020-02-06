using Application.Common.ViewModels;

namespace Application.Messages.Command.MessagesRead
{
    public class MessageReadResponse
    {
        public UserShortVm User { get; set; }
        public long MessageId { get; set; }
    }
}
