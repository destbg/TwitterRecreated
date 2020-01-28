using FluentValidation;

namespace Application.Chats.Command.CreateChat
{
    public class CreateChatValidation : AbstractValidator<CreateChatCommand>
    {
        public CreateChatValidation()
        {
            RuleFor(f => f.Username)
                .MinimumLength(6)
                .MaximumLength(32)
                .NotNull()
                .NotEmpty()
                .Matches("^[a-zA-Z0-9]*$");
        }
    }
}
