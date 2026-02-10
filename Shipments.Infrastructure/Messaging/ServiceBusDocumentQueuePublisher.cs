using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Shipments.Contracts.Messaging;

namespace Shipments.Infrastructure.Messaging;

public class ServiceBusDocumentQueuePublisher : IDocumentQueuePublisher
{
    private readonly ServiceBusSender _sender;

    public ServiceBusDocumentQueuePublisher(ServiceBusClient client, string queueOrTopicName)
    {
        _sender = client.CreateSender(queueOrTopicName);
    }

    public async Task PublishAsync(DocumentToProcessMessage message, string correlationId)
    {
        var json = JsonSerializer.Serialize(message);

        var sbMessage = new ServiceBusMessage(json)
        {
            ContentType = "application/json",
            Subject = "document-uploaded",
            CorrelationId = correlationId
        };

        sbMessage.ApplicationProperties["CorrelationId"] = correlationId;

        await _sender.SendMessageAsync(sbMessage);
    }
}
