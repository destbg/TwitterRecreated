using Application.Common.Validators;
using FluentValidation;

namespace Application.Users.Command.EditUserProfile
{
    public class EditUserProfileValidation : AbstractValidator<EditUserProfileCommand>
    {
        public EditUserProfileValidation()
        {
            RuleFor(f => f.DisplayName)
                .MinimumLength(6)
                .MaximumLength(50);

            RuleFor(f => f.Description)
                .IsValidPostText()
                .MaximumLength(250);
        }
    }
}
