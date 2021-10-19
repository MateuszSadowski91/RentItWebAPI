using RentItAPI.Models;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IEmailSender
    {
        Task SendRequestNotificationEmail(RequestEmailDto dto);

        Task SendRequestConfirmationEmail(RequestEmailDto dto);

        Task SendReservationStatusEmail(ReservationStatusEmailDto dto);

        Task SendReservationConfirmationEmail(ReservationConfirmationEmailDto dto);

        Task SendReservationNotificationEmail(ReservationConfirmationEmailDto dto);

        Task SendReservationCancellationEmail(ReservationStatusEmailDto dto);
    }
}