using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;

namespace RentItAPI.Controllers
{
    [Route("api/business")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpDelete("{businessId}")]
        public ActionResult DeleteBusiness([FromRoute] int businessId)
        {
            _businessService.Delete(businessId);
            return NoContent();
        }

        [HttpPost]
        public ActionResult CreateBusiness([FromBody] CreateBusinessDto dto)
        {
            var businessId = _businessService.Create(dto);
            return Created($"api/business/{businessId}", null);
        }
        [AllowAnonymous]
        [HttpGet("{businessId}")]
        public ActionResult GetBusinessById([FromRoute] int businessId)
        {
            var business = _businessService.GetById(businessId);
            return Ok(business);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAllBusinesses()
        {
            var businesses = _businessService.GetAll();
            return Ok(businesses);
        }
    }
}