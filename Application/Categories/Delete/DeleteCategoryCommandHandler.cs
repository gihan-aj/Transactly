using Application.Data;
using Domain.Categories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Delete
{
    internal sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(new CategoryId(request.Id));

            if(category is null)
            {
                throw new CategoryNotFoundException(new CategoryId(request.Id));
            }

            _categoryRepository.Remove(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
