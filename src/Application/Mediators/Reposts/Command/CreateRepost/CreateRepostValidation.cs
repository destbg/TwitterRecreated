using Application.Common.Validators;
using FluentValidation;

namespace Application.Reposts.Command.CreateRepost
{
    public class CreateRepostValidation : AbstractValidator<CreateRepostCommand>
    {
        public CreateRepostValidation()
        {
            RuleFor(f => f.Content)
                .MaximumLength(250)
                .IsValidPostText();
        }
    }
}
