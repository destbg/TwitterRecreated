using FluentValidation;

namespace Application.Auth.Commands.RegisterAuth
{
    public class RegisterAuthValidation : AbstractValidator<RegisterAuthCommand>
    {
        public RegisterAuthValidation()
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

            RuleFor(f => f.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(f => f.Recaptcha)
                .Matches("[0-9a-zA-Z_-]{40}")
                .NotEmpty();
        }
    }
}
