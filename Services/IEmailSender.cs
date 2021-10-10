using RentItAPI.Models;

namespace RentItAPI.Services
{
    public interface IEmailSender
    {
        void SendRequestNotificationEmail(RequestEmailDto dto);
        void SendRequestConfirmationEmail(RequestEmailDto dto);
        void SendReservationStatusEmail(ReservationStatusEmailDto dto);
        void SendReservationConfirmationEmail(ReservationConfirmationEmailDto dto);
        void SendReservationNotificationEmail(ReservationConfirmationEmailDto dto);
        void SendReservationCancellationEmail(ReservationStatusEmailDto dto);
    }  
}
