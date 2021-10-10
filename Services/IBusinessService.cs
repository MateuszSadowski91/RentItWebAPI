using RentItAPI.Models;
using System.Collections.Generic;

namespace RentItAPI.Services
{
    public interface IBusinessService
    {
        int Create(CreateBusinessDto dto);
        GetBusinessDto GetById(int businessId);
        IEnumerable<GetBusinessDto> GetAll();
        void Delete(int businessId);

    }
}
