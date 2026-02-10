using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Shipments.Contracts.Storage.Blobs;

namespace Shipments.Infrastructure.Storage;

public class AzureBlobStorage : IBlobStorage
{
    private readonly BlobServiceClient _client;

    public AzureBlobStorage(BlobServiceClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Upload Blob
    /// </summary>
    /// <param name="containerName"></param>
    /// <param name="content"></param>
    /// <param name="contentType"></param>
    /// <param name="blobName"></param>
    /// <returns></returns>
    public async Task<(string BlobName, string Url)> UploadAsync(
        string containerName,
        Stream content,
        string contentType,
        string blobName)
    {
        var container = _client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();

        var blob = container.GetBlobClient(blobName);

        await blob.UploadAsync(
            content,
            new BlobHttpHeaders { ContentType = contentType }
        );

        return (blobName, blob.Uri.ToString());
    }
}
