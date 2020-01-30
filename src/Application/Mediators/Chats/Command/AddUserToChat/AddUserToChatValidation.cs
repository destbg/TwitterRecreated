using FluentValidation;

namespace Application.Chats.Command.AddUserToChat
{
    public class AddUserToChatValidation : AbstractValidator<AddUserToChatCommand>
    {
        public AddUserToChatValidation()
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
