using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IItemService
    {
        int Create(CreateItemDto dto, int businessId);
        void DeleteItem(int itemId, int businessId);
        GetItemDto GetById(int businessId, int itemId);
        PagedResult<GetItemDto> GetAll(int businessId, ItemQuery query);
    }
}
