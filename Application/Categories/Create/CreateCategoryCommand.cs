using Application.Abstractions.Messaging;
using System;

namespace Application.Categories.Create
{
    public record CreateCategoryCommand(string Name, string? Description) : ICommand<Guid>;

    public record CreateCategoryRequest(string Name, string? Description);
}
