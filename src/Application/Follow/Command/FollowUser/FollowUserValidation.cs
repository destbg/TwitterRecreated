using FluentValidation;

namespace Application.Follow.Command.FollowUser
{
    public class FollowUserValidation : AbstractValidator<FollowUserCommand>
    {
        public FollowUserValidation()
        {
            RuleFor(f => f.Username)
                .MinimumLength(6)
                .MaximumLength(32)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9]*$");
        }
    }
}
