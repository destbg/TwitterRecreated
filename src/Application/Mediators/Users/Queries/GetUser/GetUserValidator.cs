using FluentValidation;

namespace Application.Users.Queries.GetUser
{
    public class GetUserValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserValidator()
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
