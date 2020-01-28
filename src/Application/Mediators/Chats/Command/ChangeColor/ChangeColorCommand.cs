using MediatR;

namespace Application.Chats.Command.ChangeColor
{
    public class ChangeColorCommand : IRequest
    {
        public string SelfColor { get; set; }
        public string OthersColor { get; set; }
        public long ChatId { get; set; }
    }
}
