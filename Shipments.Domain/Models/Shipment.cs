using Shipments.Domain.Enums;

namespace Shipments.Domain.Models;

/// <summary>
/// Shipment model
/// </summary>
public class Shipment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ReferenceNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Recipient { get; set; } = default!;
    public int Status { get; set; } = (int)ShipmentStatus.Created;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

