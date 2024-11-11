using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Categories;
using SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Create
{
    internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = Category.Create(request.Name, request.Description);
            _categoryRepository.Add(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
