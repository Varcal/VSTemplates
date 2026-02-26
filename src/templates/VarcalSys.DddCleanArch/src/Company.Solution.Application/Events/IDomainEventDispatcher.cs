using Company.Solution.Domain.Events;

namespace Company.Solution.Application.Events;

/// <summary>
/// Porta para despacho de domain events.
/// Definida na Application, implementada na Infrastructure.
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
