using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Entities;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmail mailSender;
        public EmailSender(IFluentEmail fluentEmail)
        {
            mailSender = fluentEmail;
        }
        public async void SendRequestNotificationEmail(RequestEmailDto dto)
        {
            var email = mailSender
            .To("rentitwebapi@gmail.com")
            .Subject($"New request from {dto.FirstName} {dto.LastName}")
            .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/RequestEmail.cshtml", dto);
 
            await email.SendAsync();
        }
        public async void SendRequestConfirmationEmail(RequestEmailDto dto)
        {
            var email = mailSender
                .To(dto.Email)
                .Subject($"Your request regarding {dto.ItemName} has been sent.")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/RequestConfirmationEmail.cshtml", dto);

            await email.SendAsync();
        }
        public async void SendReservationStatusEmail(ReservationStatusEmailDto dto)
        {
            var email = mailSender
                .To(dto.Email)
                .Subject($"Your request regarding {dto.ItemName} has been {dto.ReservationStatus}.");
            if (dto.ReservationStatus is "Accepted")
            {
                email.UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/RequestAcceptedEmail.cshtml", dto);
            }    
            email.UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/RequestRejectedEmail.cshtml", dto);

            await email.SendAsync();
        }
        public async void SendReservationConfirmationEmail(ReservationConfirmationEmailDto dto)
        {
            var email = mailSender
                .To(dto.Email)
                .Subject($"Your reservation of {dto.ItemName} has been made.")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/ReservationConfirmationEmail.cshtml", dto);

            await email.SendAsync();
        }
        public async void SendReservationNotificationEmail(ReservationConfirmationEmailDto dto)
        {
            var email = mailSender
                .To(dto.Email)
                .Subject($"New reservation from {dto.FirstName} {dto.LastName}")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/wwwroot/Emails/ReservationNotificationEmail.cshtml", dto);

            await email.SendAsync();
        }
    }
}
