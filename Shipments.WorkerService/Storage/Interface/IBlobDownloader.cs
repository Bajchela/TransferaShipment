
namespace Shipments.WorkerService.Storage.Interface
{
    public interface IBlobDownloader
    {
        Task<byte[]> DownloadAsync(string blobName, CancellationToken ct);
    }
}
