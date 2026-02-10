
using Shipments.WorkerService.Messaging;

namespace Shipments.WorkerService.Processing.Interface;

    public interface IShipmentDocumentProcessor
    {
        Task ProcessAsync(ShipmentDocumentMessage msg, CancellationToken ct);
    }

