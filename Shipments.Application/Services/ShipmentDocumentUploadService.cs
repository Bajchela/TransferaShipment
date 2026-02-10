using Microsoft.AspNetCore.Http;
using Shipments.Application.Exceptions;
using Shipments.Contracts.Interfaces.Repositories;
using Shipments.Contracts.Interfaces.Services;
using Shipments.Contracts.Messaging;
using Shipments.Contracts.Storage.Blobs;
using Shipments.Domain.Enums;
using Shipments.Domain.Models;
using System.Reflection.PortableExecutable;

namespace Shipments.Application.Services;

public class ShipmentDocumentUploadService : IShipmentDocumentUploadService
{
    private const string Header = "X-Correlation-Id";
    private readonly IHttpContextAccessor _http;
    private readonly IShipmentDocumentUploadRepository _docRepo;
    private readonly IDocumentQueuePublisher _publisher;

    public ShipmentDocumentUploadService(IShipmentDocumentUploadRepository docRepo, 
                                         IDocumentQueuePublisher publisher,
                                         IHttpContextAccessor http)
    {
        _docRepo = docRepo;
        _publisher = publisher;
        _http = http;
    }

    /// <summary>
    /// Upload blob and save metadata in database
    /// </summary>
    /// <param name="shipmentId"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="UploadFailedException"></exception>
    public async Task<(string BlobName, string Url)> UploadAsync(Guid shipmentId, IFormFile file)
    {
        var correlationId =
        _http.HttpContext?.Items[Header]?.ToString()
        ?? _http.HttpContext?.TraceIdentifier
        ?? Guid.NewGuid().ToString();

        return await _docRepo.UploadAsync(shipmentId, file, correlationId);
    }
}
