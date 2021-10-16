using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;

namespace RentItAPI.Controllers
{
    [Route("api/business/{businessId}/item/{itemId}/request")]
    [ApiController]
   // [Authorize(Roles = "Admin")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPut("accept/{requestId}")]
        public ActionResult AcceptRequest([FromRoute] int requestId, [FromBody] string? message)
        {
            _requestService.AcceptRequest(requestId, message);
            return Ok();
        }

        [HttpPut("reject/{requestId}")]
        public ActionResult RejectRequest([FromRoute] int requestId, string? message)
        {
            _requestService.RejectRequest(requestId, message);
            return Ok();
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult MakeRequest([FromRoute] int itemId, [FromBody] MakeRequestDto dto)
        {
            var newRequestId = _requestService.MakeRequest(itemId, dto);
            return Created($"api/business/businessId/item/{itemId}/request/{newRequestId}", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetAllRequests([FromQuery] RequestQuery query)
        {
            var result = _requestService.GetAll(query);
            return Ok(result);
        }

        [HttpGet("admin")]
        public ActionResult GetAllRequestsInBusiness([FromQuery] RequestQuery query, [FromRoute] int businessId)
        {
            var result = _requestService.GetAllForBusiness(query, businessId);
            return Ok(result);
        }
    }
}