﻿using Application.Abstractions.Messaging;
using Application.Categories.Get;
using Application.Data;
using Domain.Categories;
using SharedKernel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Activate
{
    internal sealed class ActivateCategoriesCommandHandler : ICommandHandler<ActivateCategoriesCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateCategoriesCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ActivateCategoriesCommand request, CancellationToken cancellationToken)
        {
            var categoryIds = new List<CategoryId>();

            foreach(var id in request.Ids)
            {
                categoryIds.Add(new CategoryId(id));
            }

            var categories = await _categoryRepository.GetRangeAsync(categoryIds);

            if(categories.Any())
            {
                foreach (var category in categories)
                {
                    if (!category.IsActive)
                    {
                        category.Activate();
                        _categoryRepository.Update(category);
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }

            if(categoryIds.Count == 1)
            {
                return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(categoryIds[0].Value));
            }

            return Result.Failure<CategoryResponse>(CategoryErrors.BulkNotFound);


        }
    }
}
