using Application.Common.Validators;
using Common;
using FluentValidation;

namespace Application.Posts.Command.CreatePost
{
    public class CreatePostValidation : AbstractValidator<CreatePostCommand>
    {
        public CreatePostValidation(IDateTime date)
        {
            RuleFor(f => f.Files)
                .FilesValidator();

            RuleFor(f => f.Content)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250)
                .IsValidPostText();

            RuleFor(f => f.Poll)
                .HasValidPoll();

            RuleFor(f => f.PollEnd)
                .Must(f => f == default || f > date.Now && f < date.Now.AddDays(8));
        }
    }
}
