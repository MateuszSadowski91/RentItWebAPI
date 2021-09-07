using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IReservationService
    {
        int MakeReservation(int itemId, MakeReservationDto dto);
        PagedResult<GetReservationDto> GetAll(ReservationQuery query);
        PagedResult<GetReservationDto> GetAllForBusiness(ReservationQuery query, int businessId);
    }
}
