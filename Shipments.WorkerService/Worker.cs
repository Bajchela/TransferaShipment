using Shipments.WorkerService.Messaging;
using Shipments.WorkerService.Processing.Interface;
using Shipments.WorkerService.Helper;

namespace Shipments.WorkerService
{
    public class Worker : BackgroundService
    {
        private const string CorrelationKey = "CorrelationId";

        private readonly IMessageReceiver _receiver;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<Worker> _logger;

        public Worker(IMessageReceiver receiver,
                        IServiceScopeFactory scopeFactory,
                        ILogger<Worker> logger)
        {
            _receiver = receiver;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ShipmentDocumentWorker started.");

            await _receiver.StartAsync(async (msg, ct) =>
            {
                var correlationId =
                   CorrelationIdHelper.TryGetCorrelationId(msg, CorrelationKey)
                   ?? Guid.NewGuid().ToString();

                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    [CorrelationKey] = correlationId
                }))
                {
                    _logger.LogInformation("Message received. CorrelationId={CorrelationId}", correlationId);

                    using var scope = _scopeFactory.CreateScope();
                    var processor = scope.ServiceProvider.GetRequiredService<IShipmentDocumentProcessor>();

                    try
                    {
                        await processor.ProcessAsync(msg, ct);
                        _logger.LogInformation("Message processed successfully. CorrelationId={CorrelationId}", correlationId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Processing failed. CorrelationId={CorrelationId}", correlationId);
                        throw; // bitno: pusti receiver da odradi DLQ/retry policy
                    }

                }
            }, stoppingToken);

            // Drži worker živim dok app radi
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ShipmentDocumentWorker stopping...");
            await _receiver.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
