using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Categories.Activate
{
    public record ActivateCategoriesCommand(List<Guid> Ids) : ICommand;
}
