using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Company.Solution.Worker.Configuration;
using Company.Solution.Worker.Consumers;

namespace Company.Solution.Worker.Infrastructure;

public sealed class RabbitMqConnectionManager(
    RabbitMqOptions options,
    IMessageConsumer consumer,
    ILogger<RabbitMqConnectionManager> logger) : IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;

    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        await ConnectWithRetryAsync(cancellationToken);
        await SetupConsumerAsync(cancellationToken);

        await Task.Delay(Timeout.Infinite, cancellationToken).ContinueWith(_ => { }, CancellationToken.None);
    }

    private async Task ConnectWithRetryAsync(CancellationToken cancellationToken)
    {
        var factory = CreateConnectionFactory();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync(cancellationToken);
                _connection.ConnectionShutdownAsync += OnConnectionShutdownAsync;
                _connection.CallbackExceptionAsync += OnCallbackExceptionAsync;

                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

                logger.LogInformation("Connected to RabbitMQ at {Host}:{Port}", options.HostName, options.Port);
                return;
            }
            catch (BrokerUnreachableException ex)
            {
                logger.LogWarning(ex, "RabbitMQ unreachable. Retrying in {Delay}s...", options.RetryDelaySeconds);
                await Task.Delay(TimeSpan.FromSeconds(options.RetryDelaySeconds), cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Failed to connect to RabbitMQ. Retrying in {Delay}s...", options.RetryDelaySeconds);
                await Task.Delay(TimeSpan.FromSeconds(options.RetryDelaySeconds), cancellationToken);
            }
        }
    }

    private async Task SetupConsumerAsync(CancellationToken cancellationToken)
    {
        if (_channel is null) return;

        await _channel.QueueDeclareAsync(
            queue: options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await _channel.BasicQosAsync(0, options.PrefetchCount, false, cancellationToken);

        var rabbitConsumer = new AsyncEventingBasicConsumer(_channel);
        rabbitConsumer.ReceivedAsync += HandleMessageAsync;

        await _channel.BasicConsumeAsync(
            queue: options.QueueName,
            autoAck: false,
            consumer: rabbitConsumer,
            cancellationToken: cancellationToken);

        logger.LogInformation("Consuming messages from queue '{Queue}'", options.QueueName);
    }

    private async Task HandleMessageAsync(object sender, BasicDeliverEventArgs ea)
    {
        var messageId = ea.BasicProperties.MessageId ?? Guid.NewGuid().ToString();
        logger.LogDebug("Received message {MessageId}", messageId);

        try
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            await consumer.ConsumeAsync(body, ea.BasicProperties, CancellationToken.None);
            await _channel!.BasicAckAsync(ea.DeliveryTag, false);
            logger.LogDebug("Message {MessageId} acknowledged", messageId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message {MessageId}. Sending nack (requeue: false)", messageId);
            await _channel!.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
        }
    }

    private ConnectionFactory CreateConnectionFactory() => new()
    {
        HostName = options.HostName,
        Port = options.Port,
        UserName = options.UserName,
        Password = options.Password,
        VirtualHost = options.VirtualHost,
        RequestedHeartbeat = TimeSpan.FromSeconds(30),
        NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
        AutomaticRecoveryEnabled = true,
        DispatchConsumersAsync = true
    };

    private Task OnConnectionShutdownAsync(object sender, ShutdownEventArgs args)
    {
        logger.LogWarning("RabbitMQ connection shutdown: {Reason}. AutoRecovery will handle reconnection.", args.ReplyText);
        return Task.CompletedTask;
    }

    private Task OnCallbackExceptionAsync(object sender, CallbackExceptionEventArgs args)
    {
        logger.LogError(args.Exception, "RabbitMQ callback exception");
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
            _channel.Dispose();
        }
        if (_connection is not null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }
    }
}
