using Application.Abstractions.Messaging;

namespace Application.Categories.Create
{
    public record CreateCategoryCommand(string Name, string? Description) : ICommand;

    public record CreateCategoryRequest(string Name, string? Description);
}
