using FluentAssertions;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Events;
using Xunit;

namespace Company.Solution.Domain.Tests;

public class ExampleAggregateTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateAggregate()
    {
        var example = ExampleAggregate.Create("Test", "Description");

        example.Id.Should().NotBeEmpty();
        example.Name.Should().Be("Test");
        example.Description.Should().Be("Description");
        example.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithValidData_ShouldRaiseDomainEvent()
    {
        var example = ExampleAggregate.Create("Test", "Description");

        example.DomainEvents.Should().HaveCount(1);
        example.DomainEvents.First().Should().BeOfType<ExampleCreatedEvent>();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowArgumentException()
    {
        var act = () => ExampleAggregate.Create("", "Description");

        act.Should().Throw<ArgumentException>();
    }
}
