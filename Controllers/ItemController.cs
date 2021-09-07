using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Entities;
using RentItAPI.Models;
using RentItAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateItem([FromRoute] int businessId, [FromBody] CreateItemDto dto)
        {
            var newItemId = _itemService.Create(dto, businessId);
            return Created($"api/business/{businessId}/item/{newItemId}", null);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{itemId}")]
        public ActionResult DeleteItem([FromRoute] int itemId, int businessId)
        {
            _itemService.DeleteItem(itemId, businessId);
            return NoContent();
        }
    }
}
