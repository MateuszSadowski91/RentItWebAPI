using RentItAPI.Entities;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IEmailSender
    {
        void SendRequestNotificationEmail(RequestEmailDto dto);
        void SendRequestConfirmationEmail(RequestEmailDto dto);
        void SendReservationStatusEmail(ReservationStatusEmailDto dto);
        void SendReservationConfirmationEmail(ReservationConfirmationEmailDto dto);
        void SendReservationNotificationEmail(ReservationConfirmationEmailDto dto);
    }  
}
