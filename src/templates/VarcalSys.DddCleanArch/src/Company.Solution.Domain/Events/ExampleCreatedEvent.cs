namespace Company.Solution.Domain.Events;

public record ExampleCreatedEvent(Guid ExampleId, string Name) : DomainEvent;
