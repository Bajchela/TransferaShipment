using Microsoft.AspNetCore.Http;

namespace Shipments.Contracts.Interfaces.Repositories;

public interface IShipmentDocumentUploadRepository
{
    Task<(string BlobName, string Url)> UploadAsync(Guid shipmentId, IFormFile file, string correlationId);
}

