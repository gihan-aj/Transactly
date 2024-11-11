using Application.Abstractions.Messaging;
using Application.Categories.Get;
using Application.Data;
using Domain.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.GetById
{
    internal sealed class GetCategoryQueryHandler : IQueryHandler<GetCategoryQuery, CategoryResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .Where(c => c.Id == new CategoryId(request.Id))
                .Select(c => new CategoryResponse(
                    c.Id.Value,
                    c.Name,
                    c.Description,
                    c.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (category is null)
            {
                return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.Id));
            }

            return category;
        }
    }
}
