using FluentValidation;

namespace Application.Auth.Commands.LoginAuth
{
    public class LoginAuthValidation : AbstractValidator<LoginAuthCommand>
    {
        public LoginAuthValidation()
        {
            RuleFor(f => f.Username)
                .MinimumLength(6)
                .MaximumLength(32)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9]*$");

            RuleFor(f => f.Password)
                .MinimumLength(12)
                .MaximumLength(64)
                .NotEmpty();

            RuleFor(f => f.Recaptcha)
                .Matches("[0-9a-zA-Z_-]{40}")
                .NotEmpty();
        }
    }
}
