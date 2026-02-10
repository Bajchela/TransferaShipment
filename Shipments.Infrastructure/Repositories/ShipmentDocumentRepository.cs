using Shipments.Contracts.Interfaces.Repositories;
using Shipments.Domain.Models;
using Shipments.Infrastructure.Persistance;

namespace Shipments.Infrastructure.Repositories;

public class ShipmentDocumentRepository : IShipmentDocumentRepository
{
    private readonly ShipmentsDbContext _db;

    public ShipmentDocumentRepository(ShipmentsDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Add shipment document
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public async Task AddAsync(ShipmentDocument doc)
    {
        _db.ShipmentDocuments.Add(doc);
        await _db.SaveChangesAsync();
    }
}
