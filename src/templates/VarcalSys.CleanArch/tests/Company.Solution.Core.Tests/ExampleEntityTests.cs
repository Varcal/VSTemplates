using FluentAssertions;
using Company.Solution.Core.Entities;
using Xunit;

namespace Company.Solution.Core.Tests;

public class ExampleEntityTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateEntity()
    {
        var entity = ExampleEntity.Create("Test", "Description");

        entity.Id.Should().NotBeEmpty();
        entity.Name.Should().Be("Test");
        entity.Description.Should().Be("Description");
        entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        entity.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowArgumentException()
    {
        var act = () => ExampleEntity.Create("", "Description");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateEntity()
    {
        var entity = ExampleEntity.Create("Test", "Description");

        entity.Update("Updated", "New Description");

        entity.Name.Should().Be("Updated");
        entity.Description.Should().Be("New Description");
        entity.UpdatedAt.Should().NotBeNull();
    }
}
