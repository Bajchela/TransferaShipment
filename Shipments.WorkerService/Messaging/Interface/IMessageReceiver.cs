namespace Shipments.WorkerService.Messaging;

public interface IMessageReceiver
{
    Task StartAsync(Func<ShipmentDocumentMessage, CancellationToken, Task> onMessage, CancellationToken ct);
    Task StopAsync(CancellationToken ct);
}