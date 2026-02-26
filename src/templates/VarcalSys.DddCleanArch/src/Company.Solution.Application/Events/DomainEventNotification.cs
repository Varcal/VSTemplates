using MediatR;
using Company.Solution.Domain.Events;

namespace Company.Solution.Application.Events;

/// <summary>
/// Adapta um IDomainEvent para INotification do MediatR.
/// Mantém o Domain desacoplado de qualquer framework de mensageria.
/// </summary>
public record DomainEventNotification<TEvent>(TEvent Event) : INotification
    where TEvent : IDomainEvent;
