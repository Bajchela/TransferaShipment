namespace Shipments.Contracts.Messaging;

public interface IDocumentQueuePublisher
{
    /// <summary>
    /// Publish
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task PublishAsync(DocumentToProcessMessage message, string correlationId);
}
