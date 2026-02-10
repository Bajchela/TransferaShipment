using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Shipments.WorkerService.Storage.Interface;

namespace Shipments.WorkerService.Storage.AzureBlob;

    public sealed class AzureBlobDownloader : IBlobDownloader
    {
        private readonly BlobContainerClient _container;

        public AzureBlobDownloader(IOptions<BlobOptions> opt)
        {
            var o = opt.Value;

            if (string.IsNullOrWhiteSpace(o.ConnectionString))
                throw new InvalidOperationException("Blob ConnectionString nije podešen.");

            if (string.IsNullOrWhiteSpace(o.ContainerName))
                throw new InvalidOperationException("Blob ContaFileNameinerName nije podešen.");

            _container = new BlobContainerClient(o.ConnectionString, o.ContainerName);
        }

        public async Task<byte[]> DownloadAsync(string blobName, CancellationToken ct)
        {
            var blob = _container.GetBlobClient(blobName);

            using var ms = new MemoryStream();
            await blob.DownloadToAsync(ms, ct);
            return ms.ToArray();
        }
    }

