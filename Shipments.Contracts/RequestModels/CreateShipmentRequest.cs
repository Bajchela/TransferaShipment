namespace Shipments.Contracts.RequestModels;

/// <summary>
/// Model for create shipment request
/// </summary>
public class CreateShipmentRequest
{
    public string ReferenceNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Recipient { get; set; } = default!;
}
