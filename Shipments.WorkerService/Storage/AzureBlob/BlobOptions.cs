namespace Shipments.WorkerService.Storage.AzureBlob
{
    public sealed class BlobOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
    }
}
