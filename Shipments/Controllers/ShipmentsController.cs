using Microsoft.AspNetCore.Mvc;
using Shipments.Contracts.Interfaces.Services;
using Shipments.Contracts.RequestModels;
using Shipments.Domain.DTOs;

namespace Shipments.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : Controller
    {
        private readonly IShipmentService _service;
        private readonly IShipmentDocumentUploadService _shipmentDoc;

        public ShipmentsController(IShipmentService service, IShipmentDocumentUploadService shipmentDoc )
        {
            _service = service;
            _shipmentDoc = shipmentDoc;
        }

        /// <summary>
        /// Create shipment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShipmentRequest request)
        {
            var created = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Get all shipments with paginations
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>List of shipments in pages result</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllShipments(int page = 1, int pageSize = 20)
        {       
            var result = await _service.GetAllAsync(page, pageSize);

            return Ok(result);
        }

        /// <summary>
        /// Get shipment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Shipment</returns>
        /// 
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var shipment = await _service.GetByIdAsync(id);                   
            return Ok(shipment);
        }

        /// <summary>
        /// Upload document
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("{shipmentId:guid}/document")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument([FromRoute] Guid shipmentId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Fajl je obavezan.");

            var (blobName, url) = await _shipmentDoc.UploadAsync(shipmentId, file);
            return Ok(new { shipmentId, blobName, url });
        }
    }
}
