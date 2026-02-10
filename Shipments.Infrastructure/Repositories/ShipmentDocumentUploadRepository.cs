using Microsoft.AspNetCore.Http;
using Shipments.Application.Exceptions;
using Shipments.Contracts.Interfaces.Repositories;
using Shipments.Contracts.Messaging;
using Shipments.Contracts.Storage.Blobs;
using Shipments.Domain.Enums;
using Shipments.Domain.Models;

namespace Shipments.Infrastructure.Repositories;

public class ShipmentDocumentUploadRepository : IShipmentDocumentUploadRepository
{
    private const string ContainerName = "shipments-documents";

    private readonly IBlobStorage _blobStorage;
    private readonly IShipmentDocumentRepository _docRepo;
    private readonly IDocumentQueuePublisher _publisher;
    private readonly IShipmentRepository _shipmentRepo;

    public ShipmentDocumentUploadRepository(
        IBlobStorage blobStorage,
        IShipmentDocumentRepository docRepo,
        IDocumentQueuePublisher publisher,
        IShipmentRepository shipmentRepo)
    {
        _blobStorage = blobStorage;
        _docRepo = docRepo;
        _publisher = publisher;
        _shipmentRepo = shipmentRepo;
    }

    /// <summary>
    /// Upload blob and save metadata in database
    /// </summary>
    /// <param name="shipmentId"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="UploadFailedException"></exception>
    public async Task<(string BlobName, string Url)> UploadAsync(Guid shipmentId, IFormFile file, string correlationId)
    {
        var exists = await _shipmentRepo.GetByIdAsync(shipmentId);
        if (exists == null)
            throw new NotFoundException();

        if (file == null || file.Length == 0)
            throw new UploadFailedException();

        var safeFileName = Path.GetFileName(file.FileName);
        var blobName = $"{shipmentId}_{safeFileName}";

        await using var stream = file.OpenReadStream();
        var uploadResult = await _blobStorage.UploadAsync(
            ContainerName, stream, file.ContentType, blobName);

        var savedBlobName = uploadResult.BlobName;
        var url = uploadResult.Url;

        var document = new ShipmentDocument
        {
            ShipmentId = shipmentId,
            BlobName = savedBlobName,
            Url = url,
            FileName = safeFileName,
            ContentType = file.ContentType,
            Size = file.Length
        };

        await _docRepo.AddAsync(document);

        await _publisher.PublishAsync(new DocumentToProcessMessage(shipmentId, savedBlobName), correlationId);

        await _shipmentRepo.UpdateStatusAsync(shipmentId, status: (int)ShipmentStatus.DocumentUploaded);

        return (savedBlobName, url);
    }
}
