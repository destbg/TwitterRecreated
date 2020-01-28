using Application.Common.Validators;
using FluentValidation;

namespace Application.Messages.Command.CreateMessage
{
    public class CreateMessageValidation : AbstractValidator<CreateMessageCommand>
    {
        public CreateMessageValidation()
        {
            RuleFor(f => f.Content)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .IsValidPostText();
        }
    }
}
