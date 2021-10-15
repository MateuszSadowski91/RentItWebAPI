using RentItAPI.Models;

namespace RentItAPI.Services
{
    public interface IReservationService
    {
        int MakeReservation(int itemId, MakeReservationDto dto);

        void CancelReservation(int reservationId, string message);

        PagedResult<GetReservationDto> GetAll(ReservationQuery query);

        PagedResult<GetReservationDto> GetAllForBusiness(ReservationQuery query, int businessId);
    }
}