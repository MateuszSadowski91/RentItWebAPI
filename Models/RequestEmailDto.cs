using System;

namespace RentItAPI.Models
{
    public class RequestEmailDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ItemName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? Message { get; set; }
        public decimal Price { get; set; }
    }
}