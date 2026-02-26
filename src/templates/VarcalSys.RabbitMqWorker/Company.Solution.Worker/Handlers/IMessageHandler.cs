using Company.Solution.Worker.Consumers;

namespace Company.Solution.Worker.Handlers;

public interface IMessageHandler
{
    Task HandleAsync(ExampleMessage message, CancellationToken cancellationToken);
}
