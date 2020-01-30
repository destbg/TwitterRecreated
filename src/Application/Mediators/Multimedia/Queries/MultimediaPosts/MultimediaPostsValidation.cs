using FluentValidation;

namespace Application.Multimedia.Queries.MultimediaPosts
{
    public class MultimediaPostsValidation : AbstractValidator<MultimediaPostsQuery>
    {
        public MultimediaPostsValidation()
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
