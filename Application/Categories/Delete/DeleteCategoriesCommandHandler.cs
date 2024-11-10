using Application.Data;
using Domain.Categories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Delete
{
    internal sealed class DeleteCategoriesCommandHandler : IRequestHandler<DeleteCategoriesCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
        {
            var categoryIds = new List<CategoryId>();

            foreach (var id in request.Ids)
            {
                categoryIds.Add(new CategoryId(id));
            }

            var categories = await _categoryRepository.GetRangeAsync(categoryIds);

            foreach (var category in categories)
            {
                _categoryRepository.Remove(category);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
