using FluentValidation;

namespace Application.Auth.Commands.MobileLogin
{
    public class MobileLoginValidation : AbstractValidator<MobileLoginCommand>
    {
        public MobileLoginValidation()
        {
            RuleFor(f => f.Username)
                .MinimumLength(6)
                .MaximumLength(32)
                .NotNull()
                .NotEmpty()
                .Matches("^[a-zA-Z0-9]*$");

            RuleFor(f => f.Password)
                .MinimumLength(12)
                .MaximumLength(64)
                .NotNull()
                .NotEmpty();
        }
    }
}
