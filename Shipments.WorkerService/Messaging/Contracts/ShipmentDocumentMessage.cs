namespace Shipments.WorkerService.Messaging;

public sealed class ShipmentDocumentMessage
{
    public Guid ShipmentId { get; init; }
    public string BlobName { get; init; } = string.Empty;
}
