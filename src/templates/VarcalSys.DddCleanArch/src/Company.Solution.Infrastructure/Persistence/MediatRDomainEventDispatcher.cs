using MediatR;
using Company.Solution.Application.Events;
using Company.Solution.Domain.Events;

namespace Company.Solution.Infrastructure.Persistence;

internal sealed class MediatRDomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;
            await mediator.Publish(notification, cancellationToken);
        }
    }
}
