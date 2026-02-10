using Shipments.Application.Shipments.Dtos;
using Shipments.Contracts.Common;
using Shipments.Domain.Models;

namespace Shipments.Contracts.Interfaces.Repositories
{
    public interface IShipmentRepository
    {
        /// <summary>
        /// Add shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns>shipment</returns>
        Task<Shipment> AddAsync(Shipment shipment);

        /// <summary>
        /// Get All async shipments data 
        /// </summary>
        /// <returns>PagedResult with list of shipment and pagination information</returns>
        Task<PagedResult<ShipmentDto>> GetAllAsync(int page, int pageSize);

        /// <summary>
        /// Get shipment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>shpiment by Id</returns>
        Task<ShipmentDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Update status
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Task UpdateStatusAsync(Guid shipmentId, int status); // ili ShipmentStatus ako koristiš enum
    }
}
