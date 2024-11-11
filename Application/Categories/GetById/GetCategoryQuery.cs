using Application.Abstractions.Messaging;
using Application.Categories.Get;
using System;

namespace Application.Categories.GetById
{
    public record GetCategoryQuery(Guid Id) : IQuery<CategoryResponse>;
}
