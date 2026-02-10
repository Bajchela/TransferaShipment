using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Shipments.WorkerService.Messaging.AzureServiceBus;

public sealed class ServiceBusMessageReceiver : IMessageReceiver
{
    private readonly ServiceBusOptions _opt;
    private readonly ILogger<ServiceBusMessageReceiver> _logger;

    private ServiceBusClient? _client;
    private ServiceBusProcessor? _processor;

    public ServiceBusMessageReceiver(IOptions<ServiceBusOptions> opt, ILogger<ServiceBusMessageReceiver> logger)
    {
        _opt = opt.Value;
        _logger = logger;
    }

    public async Task StartAsync(Func<ShipmentDocumentMessage, CancellationToken, Task> onMessage, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_opt.ConnectionString))
            throw new InvalidOperationException("ServiceBus ConnectionString nije podešen.");

        if (string.IsNullOrWhiteSpace(_opt.QueueName))
            throw new InvalidOperationException("ServiceBus QueueName nije podešen.");

        _client = new ServiceBusClient(_opt.ConnectionString);

        _processor = _client.CreateProcessor(_opt.QueueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1
        });

        _processor.ProcessMessageAsync += (args) => ProcessMessageAsync(args, onMessage);

        _processor.ProcessErrorAsync += args =>
        {
            _logger.LogError(args.Exception, "ServiceBus error. Entity={EntityPath} Source={Source}",
                args.EntityPath, args.ErrorSource);
            return Task.CompletedTask;
        };

        _logger.LogInformation("📡 Listening ServiceBus queue: {Queue}", _opt.QueueName);
        await _processor.StartProcessingAsync(ct);
    }

    public async Task StopAsync(CancellationToken ct)
    {
        if (_processor is not null)
        {
            await _processor.StopProcessingAsync(ct);
            await _processor.DisposeAsync();
        }

        if (_client is not null)
            await _client.DisposeAsync();
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args, Func<ShipmentDocumentMessage, CancellationToken, Task> onMessage)
    {
        var body = args.Message.Body.ToString();

        try
        {
            var msg = JsonSerializer.Deserialize<ShipmentDocumentMessage>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (msg is null || msg.ShipmentId == Guid.Empty || string.IsNullOrWhiteSpace(msg.BlobName))
            {
                _logger.LogWarning("Invalid SB message. Dead-letter. Body={Body}", body);
                await args.DeadLetterMessageAsync(args.Message, "Invalid payload", cancellationToken: args.CancellationToken);
                return;
            }

            await onMessage(msg, args.CancellationToken);

            await args.CompleteMessageAsync(args.Message, args.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SB message processing failed. Abandon. Body={Body}", body);
            await args.AbandonMessageAsync(args.Message, cancellationToken: args.CancellationToken);
        }
    }
}
