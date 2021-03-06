using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;
using System.Threading.Tasks;

namespace RentItAPI.Controllers
{
    [Route("invoice")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("{businessId}")]
        public async Task<ActionResult> CreateInvoiceAsync([FromRoute] int businessId, [FromBody] InvoiceModel model)
        {
            await _invoiceService.CreateAsync(businessId, model);
            return Ok();
        }

        [HttpDelete("{businessId}")]
        public async Task<ActionResult> DeleteInvoiceAsync([FromRoute] int businessId, [FromBody] DeleteInvoiceDto dto)
        {
            await _invoiceService.DeleteAsync(businessId, dto);
            return NoContent();
        }

        [HttpGet("{businessId}")]
        public async Task<ActionResult> GetInvoiceAsync([FromRoute] int businessId, [FromBody] GetInvoiceDto dto)
        {
            var fileName = dto.InvoiceName;
            var result = await _invoiceService.GetAsync(businessId, fileName);
            var contentType = result.ContentType;
            return File(result.Content, contentType, fileName);
        }

        [HttpGet("{businessId}/list")]
        public async Task<ActionResult> GetInvoiceListAsync([FromRoute] int businessId)
        {
            var result = await _invoiceService.GetListAsync(businessId);
            return Ok(result);
        }
    }
}