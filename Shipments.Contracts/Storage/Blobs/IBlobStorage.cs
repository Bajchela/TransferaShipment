namespace Shipments.Contracts.Storage.Blobs;
public interface IBlobStorage
{
    /// <summary>
    /// Upload Bloob
    /// </summary>
    /// <param name="containerName"></param>
    /// <param name="content"></param>
    /// <param name="contentType"></param>
    /// <param name="blobName"></param>
    /// <returns></returns>
    Task<(string BlobName, string Url)> UploadAsync(
        string containerName,
        Stream content,
        string contentType,
        string blobName);
}