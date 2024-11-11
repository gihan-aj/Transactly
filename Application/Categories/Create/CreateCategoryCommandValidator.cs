using Domain.Categories;
using FluentValidation;

namespace Application.Categories.Create
{
    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            //RuleFor(c => c.Name).MustAsync(
            //    async (name, _) =>
            //    {
            //        return !await categoryRepository.IsNameExistsAsync(name);
            //    })
            //    .WithMessage("The name must be unique.");

            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.Description)
                .MaximumLength(255);
        }
    }
}
