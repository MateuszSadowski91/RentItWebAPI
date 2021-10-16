using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;

namespace RentItAPI.Controllers
{
    [Route("api/business/{businessId}/item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult GetAllItems([FromRoute] int businessId, [FromQuery] ItemQuery query)
        {
            var items = _itemService.GetAll(businessId, query);
            return Ok(items);
        }

        [HttpGet("{itemId}")]
        public ActionResult GetItemById([FromRoute] int businessId, [FromRoute] int itemId)
        {
            var item = _itemService.GetById(businessId, itemId);
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateItem([FromRoute] int businessId, [FromBody] CreateItemDto dto)
        {
            var newItemId = _itemService.Create(dto, businessId);
            return Created($"api/business/{businessId}/item/{newItemId}", null);
        }

        [HttpDelete("{itemId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteItem(int businessId, [FromRoute] int itemId)
        {
            _itemService.Delete(businessId, itemId);
            return NoContent();
        }
        
        [HttpPut("{itemId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult ModifyItem([FromBody] ModifyItemDto dto, int businessId, [FromRoute] int itemId)
        {
            _itemService.Modify(dto, businessId, itemId);
            return Ok();
        }
    }
}