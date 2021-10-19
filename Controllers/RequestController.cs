using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;
using System.Threading.Tasks;

namespace RentItAPI.Controllers
{
    [Route("api/business/{businessId}/item/{itemId}/request")]
    [ApiController]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPut("accept/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptRequest([FromRoute] int requestId, [FromBody] string? message)
        {
            await _requestService.AcceptRequest(requestId, message);
            return Ok();
        }

        [HttpPut("reject/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectRequest([FromRoute] int requestId, string? message)
        {
            await _requestService.RejectRequest(requestId, message);
            return Ok();
        }
        
        [HttpPost]
        public async Task <IActionResult> MakeRequest([FromRoute] int itemId, [FromBody] MakeRequestDto dto)
        {
            var newRequestId = await _requestService.MakeRequest(itemId, dto);
            return Created($"api/business/businessId/item/{itemId}/request/{newRequestId}", null);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllRequests([FromQuery] RequestQuery query)
        {
            var result = _requestService.GetAll(query);
            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllRequestsInBusiness([FromQuery] RequestQuery query, [FromRoute] int businessId)
        {
            var result = _requestService.GetAllForBusiness(query, businessId);
            return Ok(result);
        }
    }
}