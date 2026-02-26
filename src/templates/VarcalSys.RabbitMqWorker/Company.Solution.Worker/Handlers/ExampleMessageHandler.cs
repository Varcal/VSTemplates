using Company.Solution.Worker.Consumers;

namespace Company.Solution.Worker.Handlers;

public sealed class ExampleMessageHandler(ILogger<ExampleMessageHandler> logger) : IMessageHandler
{
    public Task HandleAsync(ExampleMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling message Id={Id}, Content={Content}, Timestamp={Timestamp}",
            message.Id,
            message.Content,
            message.Timestamp);

        // TODO: Implement your business logic here

        return Task.CompletedTask;
    }
}
