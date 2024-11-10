using Application.Data;
using Domain.Categories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Update
{
    internal sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(new CategoryId(request.Id));

            if(category is null)
            {
                throw new CategoryNotFoundException(new CategoryId(request.Id));
            }

            if(category.Name != request.Name && await _categoryRepository.IsNameExistsAsync(request.Name))
            {
                throw new Exception("The name must be unique.");
            }

            category.Update(request.Name, request.Description);

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
