using System;

namespace RentItAPI.Models
{
    public class ReservationStatusEmailDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ItemName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? ReplyMessage { get; set; }
        public string ReservationStatus { get; set; }
    }
}