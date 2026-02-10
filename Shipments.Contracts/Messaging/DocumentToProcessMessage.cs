namespace Shipments.Contracts.Messaging;
    
public record DocumentToProcessMessage(Guid ShipmentId, string BlobName);

