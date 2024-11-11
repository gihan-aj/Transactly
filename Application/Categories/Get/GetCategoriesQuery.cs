using Application.Abstractions.Messaging;
using Application.Common;
using System;

namespace Application.Categories.Get
{
    public record GetCategoriesQuery(
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize) : ICommand<PagedList<CategoryResponse>>;

    public record CategoryResponse(Guid Id, string Name, string? Description, bool IsActive);
}
