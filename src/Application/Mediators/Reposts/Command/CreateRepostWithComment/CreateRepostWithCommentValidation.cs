using Application.Common.Validators;
using FluentValidation;

namespace Application.Reposts.Command.CreateRepostWithComment
{
    public class CreateRepostWithCommentValidation : AbstractValidator<CreateRepostWithCommentCommand>
    {
        public CreateRepostWithCommentValidation()
        {
            RuleFor(f => f.Content)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .IsValidPostText();
        }
    }
}
