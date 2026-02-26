using RabbitMQ.Client;

namespace Company.Solution.Worker.Consumers;

public interface IMessageConsumer
{
    Task ConsumeAsync(string body, IReadOnlyBasicProperties properties, CancellationToken cancellationToken);
}
