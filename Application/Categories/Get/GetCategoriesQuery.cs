using Application.Common;
using MediatR;
using System;

namespace Application.Categories.Get
{
    public record GetCategoriesQuery(
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize) : IRequest<PagedList<CategoryResponse>>;

    public record CategoryResponse(Guid Id, string Name, string? Description, bool IsActive);
}
