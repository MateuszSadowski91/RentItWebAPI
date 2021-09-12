using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IItemService
    {
        void Modify(ModifyItemDto dto, int businessId, int itemId);
        int Create(CreateItemDto dto, int businessId);
        void Delete(int businessId, int itemId);
        GetItemDto GetById(int businessId, int itemId);
        PagedResult<GetItemDto> GetAll(int businessId, ItemQuery query);
    }
}
