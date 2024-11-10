using System.Collections.Generic;
using System;
using Application.Abstractions.Messaging;

namespace Application.Categories.Deactivate
{
    public record DeactivateCategoriesCommand(List<Guid> Ids) : ICommand;
}
