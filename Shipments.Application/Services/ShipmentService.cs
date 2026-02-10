using AutoMapper;
using Shipments.Application.Shipments.Dtos;
using Shipments.Contracts.Common;
using Shipments.Contracts.Interfaces.Repositories;
using Shipments.Contracts.Interfaces.Services;
using Shipments.Contracts.RequestModels;
using Shipments.Domain.Models;

namespace Shipments.Application.Services
{

    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IMapper _mapper;

        public ShipmentService(IShipmentRepository shipmentRepository, IMapper mapper)
        {
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Create shipment
        /// </summary>
        /// <param name="request"></param>
        /// <returns>shipment</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ShipmentDto> CreateAsync(CreateShipmentRequest request)
        {
            // (opciono) validacije
            if (string.IsNullOrWhiteSpace(request.ReferenceNumber))
                throw new ArgumentException("Referentni broj pošiljke je obavezan.");

            if (string.IsNullOrWhiteSpace(request.Sender))
                throw new ArgumentException("Pošiljalac je obavezan.");

            if (string.IsNullOrWhiteSpace(request.Recipient))
                throw new ArgumentException("Primalac je obavezan.");

            var entity = _mapper.Map<Shipment>(request);

            var saved = await _shipmentRepository.AddAsync(entity);

            return _mapper.Map<ShipmentDto>(saved);
        }

        /// <summary>
        /// Get All shipments
        /// </summary>
        /// <returns>List of shipments</returns>
        public async Task<PagedResult<ShipmentDto>> GetAllAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            return  await _shipmentRepository.GetAllAsync(page, pageSize);
        }

        /// <summary>
        /// Get Shipment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Shipment</returns>
        public async Task<ShipmentDto?> GetByIdAsync(Guid id)
        {
            return await _shipmentRepository.GetByIdAsync(id);
        }
    }
}
