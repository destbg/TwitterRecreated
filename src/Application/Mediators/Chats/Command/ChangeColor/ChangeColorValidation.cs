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
                .Length(7)
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");

            RuleFor(f => f.SelfColor)
                .NotNull()
                .NotEmpty()
                .Length(7)
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
        }
    }
}
