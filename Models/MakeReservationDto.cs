using System;

namespace RentItAPI.Models
{
    public class MakeReservationDto
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? Message { get; set; }
    }
}