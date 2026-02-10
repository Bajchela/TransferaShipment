using Shipments.Domain.Models;

namespace Shipments.Contracts.Interfaces.Repositories
{
    public interface IShipmentDocumentRepository
    {        
        /// <summary>
        /// Add Document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        Task AddAsync(ShipmentDocument doc);
    }
}
