using Shipments.Contracts.Storage.Blobs;

namespace Shipments.Infrastructure.Storage;

public class FakeBlobStorage : IBlobStorage
{
    public async Task<(string BlobName, string Url)> UploadAsync(
        string containerName,
        Stream content,
        string contentType,
        string blobName)
    {
        // snimi lokalno, npr. u ./fake-blob/
        Directory.CreateDirectory("fake-blob");
        var localPath = Path.Combine("fake-blob", blobName.Replace("/", "__"));

        await using var fs = File.Create(localPath);
        await content.CopyToAsync(fs);

        var url = $"file:///{Path.GetFullPath(localPath).Replace("\\", "/")}";
        Console.WriteLine($"[FAKE BLOB] Saved to {localPath} ({contentType})");

        return (blobName, url);
    }
}
