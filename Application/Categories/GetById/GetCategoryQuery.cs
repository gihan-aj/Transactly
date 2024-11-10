using Application.Categories.Get;
using MediatR;
using System;

namespace Application.Categories.GetById
{
    public record GetCategoryQuery(Guid Id) : IRequest<CategoryResponse>;
}
