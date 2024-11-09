using MediatR;
using System;

namespace Domain.Primitives
{
    public record DomainEvent(Guid Id) : INotification;
}
