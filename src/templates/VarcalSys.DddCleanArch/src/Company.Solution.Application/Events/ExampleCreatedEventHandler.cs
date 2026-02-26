using MediatR;
using Company.Solution.Domain.Events;

namespace Company.Solution.Application.Events;

internal sealed class ExampleCreatedEventHandler
    : INotificationHandler<DomainEventNotification<ExampleCreatedEvent>>
{
    public Task Handle(
        DomainEventNotification<ExampleCreatedEvent> notification,
        CancellationToken cancellationToken)
    {
        // TODO: reagir ao evento (ex: enviar e-mail, atualizar read model, etc.)
        _ = notification.Event;
        return Task.CompletedTask;
    }
}
