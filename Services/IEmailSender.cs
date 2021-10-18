using RentItAPI.Models;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IEmailSender
    {
        Task SendRequestNotificationEmail(RequestEmailDto dto);

        Task SendRequestConfirmationEmail(RequestEmailDto dto);

        void SendReservationStatusEmail(ReservationStatusEmailDto dto);

        void SendReservationConfirmationEmail(ReservationConfirmationEmailDto dto);

        void SendReservationNotificationEmail(ReservationConfirmationEmailDto dto);

        void SendReservationCancellationEmail(ReservationStatusEmailDto dto);
    }
}