using Shipments.Contracts.Messaging;

namespace Shipments.Infrastructure.Messaging;

public class FakeDocumentQueuePublisher : IDocumentQueuePublisher
{
    /// <summary>
    /// Fake service
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task PublishAsync(DocumentToProcessMessage message, string correlationId)
    {
        Console.WriteLine(
            $"[FAKE SERVICE BUS] documents-to-process -> ShipmentId={message.ShipmentId}, BlobName={message.BlobName}"
        );
        return Task.CompletedTask;
    }
}
