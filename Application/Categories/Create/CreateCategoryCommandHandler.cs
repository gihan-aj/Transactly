using Application.Data;
using Domain.Categories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Create
{
    internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = Category.Create(request.Name, request.Description);
            _categoryRepository.Add(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
