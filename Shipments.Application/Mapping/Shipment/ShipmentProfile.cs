using AutoMapper;
using Shipments.Application.Shipments.Dtos;
using Shipments.Contracts.RequestModels;
using Shipments.Domain.Models;

namespace Shipments.Application.Mapping
{
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            CreateMap<Shipment, ShipmentDto>();

            CreateMap<CreateShipmentRequest, Shipment>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore());
        }
    }
}
