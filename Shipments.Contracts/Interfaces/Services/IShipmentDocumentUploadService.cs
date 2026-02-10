using Microsoft.AspNetCore.Http;

namespace Shipments.Contracts.Interfaces.Services;

public interface IShipmentDocumentUploadService
{
    Task<(string BlobName, string Url)> UploadAsync(Guid shipmentId, IFormFile file);
}

