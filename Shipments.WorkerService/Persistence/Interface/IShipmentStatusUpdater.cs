namespace Shipments.WorkerService.Persistence.Interface;

    public interface IShipmentStatusUpdater
    {
        Task MarkProcessedAsync(Guid shipmentId, CancellationToken ct);
    }

