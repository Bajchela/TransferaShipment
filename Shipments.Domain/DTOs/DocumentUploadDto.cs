namespace Shipments.Domain.DTOs
{
    public class DocumentUploadDto
    {
        public Guid ShipmentId { get; set; }
        public string BlobName { get; set; } = default!;
        public string Url { get; set; } = default!;
    }
}
