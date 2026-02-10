namespace Shipments.WorkerService.Messaging.AzureServiceBus
{
    public sealed class ServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
    }
}
