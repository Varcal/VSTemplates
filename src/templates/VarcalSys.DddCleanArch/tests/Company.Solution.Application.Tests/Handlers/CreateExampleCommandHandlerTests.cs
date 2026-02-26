using FluentAssertions;
using NSubstitute;
using Company.Solution.Application.Commands;
using Company.Solution.Application.Handlers;
using Company.Solution.Domain.Entities;
using Company.Solution.Domain.Repositories;

namespace Company.Solution.Application.Tests.Handlers;

public class CreateExampleCommandHandlerTests
{
    private readonly IRepository<ExampleAggregate> _repository = Substitute.For<IRepository<ExampleAggregate>>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnDto()
    {
        var handler = new CreateExampleCommandHandler(_repository, _unitOfWork);
        var command = new CreateExampleCommand("Test", "Description");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        result.Description.Should().Be("Description");
        result.Id.Should().NotBeEmpty();
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
