using Application.Abstractions.Messaging;
using Application.Categories.Get;
using Application.Data;
using Domain.Categories;
using SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Create
{
    internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if(await _categoryRepository.IsNameExistsAsync(request.Name))
            {
                return Result.Failure<Guid>(CategoryErrors.DuplicateName(request.Name));
            }

            var category = Category.Create(request.Name, request.Description);
            _categoryRepository.Add(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return category.Id.Value;
        }
    }
}
