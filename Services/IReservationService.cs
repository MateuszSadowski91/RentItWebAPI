using RentItAPI.Models;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IReservationService
    {
        Task<int> MakeReservation(int itemId, MakeReservationDto dto);

        Task CancelReservation(int reservationId, string message);

        PagedResult<GetReservationDto> GetAll(ReservationQuery query);

        PagedResult<GetReservationDto> GetAllForBusiness(ReservationQuery query, int businessId);
    }
}