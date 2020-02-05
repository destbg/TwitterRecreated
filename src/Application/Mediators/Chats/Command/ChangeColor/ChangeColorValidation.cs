using FluentValidation;

namespace Application.Chats.Command.ChangeColor
{
    public class ChangeColorValidation : AbstractValidator<ChangeColorCommand>
    {
        public ChangeColorValidation()
        {
            RuleFor(f => f.OthersColor)
                .NotNull()
                .NotEmpty()
                .Length(6)
                .Matches("^[0-9A-Fa-f]{6}$");

            RuleFor(f => f.SelfColor)
                .NotNull()
                .NotEmpty()
                .Length(6)
                .Matches("^[0-9A-Fa-f]{6}$");
        }
    }
}
