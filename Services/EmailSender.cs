using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using RentItAPI.Models;
using System.IO;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmailFactory mailSender;
        private readonly IConfiguration _configuration;

        public EmailSender(IFluentEmailFactory fluentEmail, IConfiguration configuration)
        {
            mailSender = fluentEmail;
            _configuration = configuration;
        }

        public async Task SendRequestNotificationEmail(RequestEmailDto dto)
        {
   
            var notificationAddress = GetNotificationAddress();
            var email = mailSender
            .Create()
            .To(notificationAddress)
            .Subject($"New request from {dto.FirstName} {dto.LastName}")
            .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/RequestEmail.cshtml", dto);

            await email.SendAsync();
        }

        public async Task SendRequestConfirmationEmail(RequestEmailDto dto)
        {
            var email = mailSender
                .Create()
                .To(dto.Email)
                .Subject($"Your request regarding {dto.ItemName} has been sent.")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/RequestConfirmationEmail.cshtml", dto);

            await email.SendAsync();
        }

        public async Task SendReservationStatusEmail(ReservationStatusEmailDto dto)
        {
            var email = mailSender
                .Create()
                .To(dto.Email)
                .Subject($"Your request regarding {dto.ItemName} has been {dto.ReservationStatus}.");
            if (dto.ReservationStatus is "Accepted")
            {
                email.UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/RequestAcceptedEmail.cshtml", dto);
            }
            email.UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/RequestRejectedEmail.cshtml", dto);

            await email.SendAsync();
        }

        public async Task SendReservationConfirmationEmail(ReservationConfirmationEmailDto dto)
        {
            var email = mailSender
                .Create()
                .To(dto.Email)
                .Subject($"Your reservation of {dto.ItemName} has been made.")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/ReservationConfirmationEmail.cshtml", dto);

            await email.SendAsync();
        }

        public async Task SendReservationNotificationEmail(ReservationConfirmationEmailDto dto)
        {
            var notificationAddress = GetNotificationAddress();
            var email = mailSender
                .Create()
                .To(notificationAddress)
                .Subject($"New reservation from {dto.FirstName} {dto.LastName}")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/ReservationNotificationEmail.cshtml", dto);

            await email.SendAsync();
        }

        public async Task SendReservationCancellationEmail(ReservationStatusEmailDto dto)
        {
            var email = mailSender
                .Create()
                .To(dto.Email)
                .Subject($"Your reservation has been canceled.")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/ReservationCancellationEmail.cshtml", dto);

            await email.SendAsync();
        }
        private string GetNotificationAddress()
        {
            var notificationAddress = _configuration.GetSection("Gmail")["Sender"];

            return notificationAddress;
        }
    }
}