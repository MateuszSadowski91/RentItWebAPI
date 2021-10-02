using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Controllers
{
    [Route("invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateInvoice([FromBody] InvoiceModel model)
        {
            _invoiceService.Create(model);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteInvoice([FromBody] DeleteInvoiceDto dto)
        {
            _invoiceService.Delete(dto);
            return NoContent();
        }
    }
}
