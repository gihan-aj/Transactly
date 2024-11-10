using Application.Abstractions.Messaging;
using System;

namespace Application.Categories.Update
{
    public record UpdateCategoryCommand(Guid Id, string Name, string? Description) : ICommand;

    public record UpdateCategoryRequest(string Name, string? Description);
}
