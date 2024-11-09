using Domain.Categories;
using FluentValidation;

namespace Application.Categories.Create
{
    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
        {
            RuleFor(c => c.Name).MustAsync(
                async (name, _) =>
                {
                    return !await categoryRepository.IsNameUniqueAsync(name);
                })
                .WithMessage("The name must be unique.");
        }
    }
}
