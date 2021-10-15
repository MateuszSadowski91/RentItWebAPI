using System;

namespace RentItAPI.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int? CreatedById { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        private string? Message { get; set; }
        public Status RequestStatus { get; set; } = Status.Pending;
        public virtual Business Business { get; set; }
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}