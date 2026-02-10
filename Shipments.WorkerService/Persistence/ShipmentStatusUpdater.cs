using Microsoft.EntityFrameworkCore;
using Shipments.Domain.Enums;
using Shipments.WorkerService.Persistence.Interface;
using Shipments.WorkerService.Persistence.Worker;

public sealed class ShipmentStatusUpdater : IShipmentStatusUpdater
{
    private readonly WorkerDbContext _db;

    public ShipmentStatusUpdater(WorkerDbContext db)
    {
        _db = db;
    }

    public async Task MarkProcessedAsync(Guid shipmentId, CancellationToken ct)
    {
        var shipment = await _db.Shipments
            .FirstOrDefaultAsync(x => x.Id == shipmentId, ct);

        if (shipment == null)
            return;

        // Status je int u bazi
        shipment.Status = (int)ShipmentStatus.DocumentProcessed;

        await _db.SaveChangesAsync(ct);
    }
}
