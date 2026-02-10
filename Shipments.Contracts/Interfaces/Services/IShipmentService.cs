using Shipments.Application.Shipments.Dtos;
using Shipments.Contracts.Common;
using Shipments.Contracts.RequestModels;

namespace Shipments.Contracts.Interfaces.Services
{
    public interface IShipmentService
    {
        /// <summary>
        /// Create shipment
        /// </summary>
        /// <param name="request"></param>
        /// <returns>shipment</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<ShipmentDto> CreateAsync(CreateShipmentRequest request);

        /// <summary>
        /// Get All shipments
        /// </summary>
        /// <returns>List of shipments</returns>
        Task<PagedResult<ShipmentDto>> GetAllAsync(int page, int pageSize);

        /// <summary>
        /// Get Shipment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Shipment</returns>
        Task<ShipmentDto?> GetByIdAsync(Guid id);
    }
}
