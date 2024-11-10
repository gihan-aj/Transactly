using Application.Abstractions.Messaging;
using System;

namespace Application.Categories.Delete
{
    public record DeleteCategoryCommand(Guid Id) : ICommand;
}
