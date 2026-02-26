using Company.Solution.Domain.Events;

namespace Company.Solution.Domain.Entities;

public class ExampleAggregate : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private ExampleAggregate() { }

    public static ExampleAggregate Create(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var aggregate = new ExampleAggregate
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };

        aggregate.AddDomainEvent(new ExampleCreatedEvent(aggregate.Id, name));
        return aggregate;
    }

    public void Update(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Description = description;
    }
}
