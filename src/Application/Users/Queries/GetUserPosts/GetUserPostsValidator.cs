using FluentValidation;

namespace Application.Users.Queries.GetUserPosts
{
    public class GetUserPostsValidator : AbstractValidator<GetUserPostsQuery>
    {
        public GetUserPostsValidator()
        {
            RuleFor(f => f.Username)
                .MinimumLength(6)
                .MaximumLength(32)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9]*$");
        }
    }
}
