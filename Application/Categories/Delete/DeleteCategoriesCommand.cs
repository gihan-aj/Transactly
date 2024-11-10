using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Categories.Delete
{
    public record DeleteCategoriesCommand(List<Guid> Ids) : ICommand;
}
