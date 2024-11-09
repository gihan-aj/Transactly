using Application.Common;
using Application.Data;
using Domain.Categories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Get
{
    internal sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedList<CategoryResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedList<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Category> categoryQuery = _context.Categories;

            // Filtering
            if(!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                categoryQuery = categoryQuery
                    .Where(c => c.Name.Contains(request.SearchTerm));
                // non-clustered indexes would be needed to speed up the process
            }

            // Sorting
            if(request.SortOrder?.ToLower() == "desc")
            {
                categoryQuery = categoryQuery
                    .OrderByDescending(GetSortProperty(request));
            }
            else
            {
                categoryQuery = categoryQuery
                    .OrderBy(GetSortProperty(request));
            }

            // Selecting
            var categoryResponsesQuery = categoryQuery
                .Select(c => new CategoryResponse(
                    c.Id.Value,
                    c.Name,
                    c.Description,
                    c.IsActive));

            var categories = await PagedList<CategoryResponse>.CreateAsync(
                categoryResponsesQuery,
                request.Page,
                request.PageSize,
                cancellationToken);

            return categories;
        }

        // Provide the sort property to the query
        private static Expression<Func<Category, object>> GetSortProperty(GetCategoriesQuery request)
        {
            return request.SortColumn?.ToLower() switch
            {
                "name" => category => category.Name,
                _ => category => category.Id
            };
        }
    }
}
