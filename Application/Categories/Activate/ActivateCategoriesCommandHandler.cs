using Application.Data;
using Domain.Categories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Activate
{
    internal sealed class ActivateCategoriesCommandHandler : IRequestHandler<ActivateCategoriesCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ActivateCategoriesCommand request, CancellationToken cancellationToken)
        {
            var categoryIds = new List<CategoryId>();

            foreach(var id in request.Ids)
            {
                categoryIds.Add(new CategoryId(id));
            }

            var categories = await _categoryRepository.GetRangeAsync(categoryIds);

            foreach (var category in categories)
            {
                if (!category.IsActive)
                {
                    category.Activate();
                    _categoryRepository.Update(category);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
