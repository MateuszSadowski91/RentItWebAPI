using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IBusinessService
    {
        int Create(CreateBusinessDto dto);
        GetBusinessDto GetById(int businessId);
        IEnumerable<GetBusinessDto> GetAll();
    }
}
