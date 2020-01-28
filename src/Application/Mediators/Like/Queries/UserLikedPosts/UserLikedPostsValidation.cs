using FluentValidation;

namespace Application.Like.Queries.UserLikedPosts
{
    public class UserLikedPostsValidation : AbstractValidator<UserLikedPostsQuery>
    {
        public UserLikedPostsValidation()
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
