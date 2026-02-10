using Shipments.WorkerService.Messaging;
using Shipments.WorkerService.Persistence.Interface;
using Shipments.WorkerService.Processing.Interface;
using Shipments.WorkerService.Storage.Interface;

namespace Shipments.WorkerService.Processing
{
    public sealed class ShipmentDocumentProcessor : IShipmentDocumentProcessor
    {
        private readonly IBlobDownloader _blob;
        private readonly IShipmentStatusUpdater _shipmentUpdate;
        private readonly ILogger<ShipmentDocumentProcessor> _logger;

        public ShipmentDocumentProcessor(
            IBlobDownloader blob,
            IShipmentStatusUpdater shipmentUpdate,
            ILogger<ShipmentDocumentProcessor> logger)
        {
            _blob = blob;
            _shipmentUpdate = shipmentUpdate;
            _logger = logger;
        }

        public async Task ProcessAsync(ShipmentDocumentMessage msg, CancellationToken ct)
        {
            _logger.LogInformation("Start processing: ShipmentId={ShipmentId}, Blob={Blob}",
                msg.ShipmentId, msg.BlobName);

            var bytes = await _blob.DownloadAsync(msg.BlobName, ct);
            _logger.LogInformation(" Blob downloaded: {Bytes} bytes", bytes.Length);

            await Task.Delay(TimeSpan.FromSeconds(2), ct);
            _logger.LogInformation("Simulated processing finished.");

            await _shipmentUpdate.MarkProcessedAsync(msg.ShipmentId, ct);
            _logger.LogInformation("Status updated to Processed: ShipmentId={ShipmentId}", msg.ShipmentId);
        }
    }
}
