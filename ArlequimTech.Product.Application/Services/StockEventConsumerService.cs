using System.Text;
using System.Text.Json;
using ArlequimTech.Core.Messaging;
using ArlequimTech.Core.Messaging.Events;
using ArlequimTech.Product.Application.Handlers.Contracts;
using ArlequimTech.Product.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ArlequimTech.Product.Application.Services;

public class StockEventConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqSettings _settings;
    private IConnection _connection;
    private IChannel _channel;

    public StockEventConsumerService(
        IServiceScopeFactory scopeFactory,
        IOptions<RabbitMqSettings> settings)
    {
        _scopeFactory = scopeFactory;
        _settings = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitializeRabbitMqAsync();

        await _channel!.QueueDeclareAsync(
            queue: QueueNames.AddStockQuantityToProduct,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var stockEvent = JsonSerializer.Deserialize<AddStockQuantityToProductEvent>(json);

                if (stockEvent is not null)
                {
                    await HandleEventAsync(stockEvent);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueNames.AddStockQuantityToProduct,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleEventAsync(AddStockQuantityToProductEvent stockEvent)
    {
        using var scope = _scopeFactory.CreateScope();
        var productHandler = scope.ServiceProvider.GetRequiredService<IProductHandler>();

        var command = new AddStockQuantityToProductCommand(stockEvent.ProductId, stockEvent.Quantity);
        
        await productHandler.Handle(command);
    }

    private async Task InitializeRabbitMqAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
            await _channel.CloseAsync(cancellationToken);
        if (_connection is not null)
            await _connection.CloseAsync(cancellationToken);

        await base.StopAsync(cancellationToken);
    }
}
