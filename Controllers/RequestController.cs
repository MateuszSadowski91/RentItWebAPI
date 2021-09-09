using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;

namespace RentItAPI.Controllers
{
    [Route("api/business/{businessId}/item/{itemId}/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("accept")]
        public ActionResult AcceptRequest([FromRoute]int requestId, [FromBody] string? message)
        {
            _requestService.AcceptRequest(requestId, message);
            return Ok();
        }

        [Authorize(Roles="Admin")]
        [HttpPut("reject")]
        public ActionResult RejectRequest([FromRoute] int requestId, string? message)
        {
            _requestService.RejectRequest(requestId, message);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public ActionResult MakeRequest([FromRoute] int itemId, [FromBody] MakeRequestDto dto)
        {
            var newRequestId = _requestService.MakeRequest(itemId, dto);
            return Created($"api/business/businessId/item/{itemId}/request/{newRequestId}", null);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetAllRequests([FromQuery] RequestQuery query)
        {
            var result = _requestService.GetAll(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public ActionResult GetAllRequestsInBusiness([FromQuery] RequestQuery query, [FromRoute] int businessId)
        {
            var result = _requestService.GetAllForBusiness(query, businessId);
            return Ok(result);
        }
    }
}

        
   

