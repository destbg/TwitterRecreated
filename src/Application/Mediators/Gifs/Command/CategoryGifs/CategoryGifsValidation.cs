using FluentValidation;

namespace Application.Gifs.Command.CategoryGifs
{
    public class CategoryGifsValidation : AbstractValidator<CategoryGifsCommand>
    {
        public CategoryGifsValidation()
        {
            RuleFor(f => f.Tag)
                .NotNull()
                .NotEmpty()
                .Matches(@"^[a-zA-Z\s]*$");
        }
    }
}
