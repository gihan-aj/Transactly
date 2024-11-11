using Application.Abstractions.Messaging;
using Application.Categories.Get;
using Application.Data;
using Domain.Categories;
using SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Update
{
    internal sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(new CategoryId(request.Id));

            if(category is null)
            {
                return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.Id));
            }

            if(category.Name != request.Name && await _categoryRepository.IsNameExistsAsync(request.Name))
            {
                return Result.Failure<CategoryResponse>(CategoryErrors.DuplicateName(request.Name));
            }

            category.Update(request.Name, request.Description);

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
