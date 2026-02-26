using Company.Solution.Worker.Infrastructure;

namespace Company.Solution.Worker.Workers;

public sealed class RabbitMqWorker(
    RabbitMqConnectionManager connectionManager,
    ILogger<RabbitMqWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RabbitMQ Worker starting...");

        try
        {
            await connectionManager.StartConsumingAsync(stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("RabbitMQ Worker stopping gracefully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "RabbitMQ Worker failed with unhandled exception.");
            throw;
        }
        finally
        {
            await connectionManager.DisposeAsync();
        }
    }
}
