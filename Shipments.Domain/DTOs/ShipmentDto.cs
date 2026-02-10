using Shipments.Domain.Enums;

namespace Shipments.Application.Shipments.Dtos;

public class ShipmentDto
{
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Recipient { get; set; } = default!;

    public ShipmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
