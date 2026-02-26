using RabbitMQ.Client;
using System.Text.Json;
using Company.Solution.Worker.Handlers;

namespace Company.Solution.Worker.Consumers;

public sealed class ExampleConsumer(
    IMessageHandler messageHandler,
    ILogger<ExampleConsumer> logger) : IMessageConsumer
{
    public async Task ConsumeAsync(string body, IReadOnlyBasicProperties properties, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing message: {Body}", body);

        try
        {
            var message = JsonSerializer.Deserialize<ExampleMessage>(body)
                ?? throw new InvalidOperationException("Failed to deserialize message body.");

            await messageHandler.HandleAsync(message, cancellationToken);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Invalid message format. Body: {Body}", body);
            throw;
        }
    }
}

public record ExampleMessage(string Id, string Content, DateTime Timestamp);
