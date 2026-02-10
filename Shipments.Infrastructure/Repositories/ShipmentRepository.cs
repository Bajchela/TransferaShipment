using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shipments.Application.Exceptions;
using Shipments.Application.Shipments.Dtos;
using Shipments.Contracts.Common;
using Shipments.Contracts.Interfaces.Repositories;
using Shipments.Domain.Enums;
using Shipments.Domain.Models;
using Shipments.Infrastructure.Persistance;

namespace Shipments.Infrastructure.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ShipmentsDbContext _db;
        private readonly IMapper _mapper;

        public ShipmentRepository(ShipmentsDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// Add shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns>shipment</returns>
        public async Task<Shipment> AddAsync(Shipment shipment)
        {
            shipment.Status = (int)ShipmentStatus.Created;
            _db.Shipments.Add(shipment);
            await _db.SaveChangesAsync();
            return shipment;
        }

        /// <summary>
        /// Get All async shipments data 
        /// </summary>
        /// <returns>PagedResult with list of shipment and pagination information</returns>
        public async Task<PagedResult<ShipmentDto>> GetAllAsync(int page, int pageSize)
        {         
            var baseQuery = _db.Shipments.AsNoTracking().OrderByDescending(x => x.CreatedAt);

            var totalCount = await baseQuery.CountAsync();

            var allShipments = await baseQuery
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ProjectTo<ShipmentDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync();

            return new PagedResult<ShipmentDto>
            {
                Items = allShipments,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        /// <summary>
        /// Get shipment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>shpiment by Id</returns>
        public async Task<ShipmentDto?> GetByIdAsync(Guid id)
        {
            var shipment = await _db.Shipments
                               .AsNoTracking()
                               .Where(x => x.Id == id)
                               .ProjectTo<ShipmentDto>(_mapper.ConfigurationProvider)
                               .SingleOrDefaultAsync();

            if (shipment == null)
                throw new NotFoundException();

            return shipment;
        }

        /// <summary>
        /// Update status
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task UpdateStatusAsync(Guid shipmentId, int status)
        {
            var shipment = await _db.Shipments.FirstOrDefaultAsync(x => x.Id == shipmentId);
            if (shipment == null)
                throw new NotFoundException();

            shipment.Status = status;          
            shipment.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();
        }
    }
}
